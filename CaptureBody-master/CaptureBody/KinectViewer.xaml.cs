using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Microsoft.Kinect.Tools;
using System.IO;
using System.Collections;
//using CaptureAngles.Core;

namespace CaptureBody
{
    /// <summary>
    /// Interaction logic for KinectViewer.xaml
    /// </summary>
    public partial class KinectViewer : UserControl
    {
        #region Members

        // Representa o Kinect
        private KinectSensor _kinect = null;

        // Leitor de frames para visualisação na tela
        MultiSourceFrameReader _reader;

        //Emula o Kinect
        KStudioClient _client = null;

        //Receber o Arquivo .xef
        KStudioPlayback _playback = null;

        //Mapear de um tipo de ponto para outro
        private CoordinateMapper _coordinateMapper = null;

        //
        ushort[] _depthData;


        /* Body */

        // Todos os Objetos/Corpos rastreados
        private Body[] _bodies = null;

        private Body bodyTracked = null;


        /* Define Joints */

        // Torso Joints
        private Joint Head;
        private Joint Neck;
        private Joint SpineShoulder;
        private Joint SpineMid;
        private Joint SpineBase;

        // Left Joints
        private Joint ShoulderLeft;
        private Joint ElbowLeft;
        private Joint WristLeft;
        private Joint HipLeft;
        private Joint KneeLeft;
        private Joint AnkleLeft;
        private Joint FootLeft;

        // Right Joints
        private Joint ShoulderRight;
        private Joint ElbowRight;
        private Joint WristRight;
        public Joint HipRight;
        public Joint KneeRight;
        public Joint AnkleRight;
        private Joint FootRight;

        private int countFrames;

        private String filePathSequanceFrames;

        // Texte Time
        public TextBlock time = new TextBlock();

        // Evento de alterações
        public event EventHandler ChangesMethods;

        #endregion

        #region Contructors

        public KinectViewer()
        {
            InitializeComponent();

            filePathSequanceFrames = "";
            countFrames = 0;
        }

        #endregion

        #region Kinect
        //Conecta com o Kinect
        private void InitializeKinect()
        {
            _kinect = KinectSensor.GetDefault();

            if (_kinect == null) return;

            _kinect.Open();

            _coordinateMapper = _kinect.CoordinateMapper;

        }

        //Fecha a conexão com o Kinect
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_kinect != null)
            {
                _kinect.Close();
            }
        }

        #endregion

        #region Reader

        private void InitializeReader()
        {
            _reader = _kinect.OpenMultiSourceFrameReader(FrameSourceTypes.Color |
                                             FrameSourceTypes.Depth |
                                             FrameSourceTypes.Infrared |
                                             FrameSourceTypes.Body);
            _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
        }

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            // Gera um sinal
            OnChangesMethods(EventArgs.Empty);

            // Get a reference to the multi-frame
            var reference = e.FrameReference.AcquireFrame();

            if (IsPlaying())
            {
                time.Text = timePlayback();
            }
            

            // Open color frame
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {

                    if (this.Visualization == Visualization.Color)
                    {

                        Camera.Source = ToBitmap(frame);

                    }
                }
            }

            // Open depth frame
            using (var frame = reference.DepthFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (this.Visualization == Visualization.Depth)
                    {

                        Camera.Source = ToBitmap(frame);

                    }
                }
            }

            // Open infrared frame
            using (var frame = reference.InfraredFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (this.Visualization == Visualization.Infrared)
                    {
                        Camera.Source = ToBitmap(frame);

                    }
                }
            }

            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    _bodies = new Body[frame.BodyFrameSource.BodyCount];

                    frame.GetAndRefreshBodyData(_bodies);

                    foreach (var body in _bodies)
                    {
                        if (body != null)
                        {
                            if (body.IsTracked)
                            {
                                this.bodyTracked = body;
                                Skeleton.Children.Clear();
                                updateJoint(body);
                                DrawBody(body);

                            }
                        }
                        if (countFrames > 0)
                        {
                            captureFrame(_depthData, (BitmapSource)(Camera.Source), filePathSequanceFrames + "depthFrame_" + _playback.CurrentRelativeTime.Minutes + "-" +
                                _playback.CurrentRelativeTime.Seconds + "-" +
                                _playback.CurrentRelativeTime.Milliseconds);
                            countFrames++;
                        }
                        if (countFrames > 20)
                        {
                            Pause();
                            countFrames = 0;
                        }
                    }
                }
            }
        }

        private ImageSource ToBitmap(ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;

            byte[] pixels = new byte[width * height * ((PixelFormats.Bgr32.BitsPerPixel + 7) / 8)];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            int stride = width * PixelFormats.Bgr32.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgr32, null, pixels, stride);
        }

        private ImageSource ToBitmap(DepthFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;

            ushort minDepth = frame.DepthMinReliableDistance;
            ushort maxDepth = frame.DepthMaxReliableDistance;

            _depthData = new ushort[width * height];
            byte[] pixelData = new byte[width * height * (PixelFormats.Bgr32.BitsPerPixel + 7) / 8];

            frame.CopyFrameDataToArray(_depthData);

            int colorIndex = 0;
            for (int depthIndex = 0; depthIndex < _depthData.Length; ++depthIndex)
            {
                ushort depth = _depthData[depthIndex];
                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                pixelData[colorIndex++] = intensity; // Blue
                pixelData[colorIndex++] = intensity; // Green
                pixelData[colorIndex++] = intensity; // Red

                ++colorIndex;
            }

            int stride = width * PixelFormats.Bgr32.BitsPerPixel / 8;


            return BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgr32, null, pixelData, stride);
        }

        private ImageSource ToBitmap(InfraredFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;

            ushort[] infraredData = new ushort[width * height];
            byte[] pixelData = new byte[width * height * (PixelFormats.Bgr32.BitsPerPixel + 7) / 8];

            frame.CopyFrameDataToArray(infraredData);

            int colorIndex = 0;
            for (int infraredIndex = 0; infraredIndex < infraredData.Length; ++infraredIndex)
            {
                ushort ir = infraredData[infraredIndex];
                byte intensity = (byte)(ir >> 8);

                pixelData[colorIndex++] = intensity; // Blue
                pixelData[colorIndex++] = intensity; // Green   
                pixelData[colorIndex++] = intensity; // Red

                ++colorIndex;
            }

            int stride = width * PixelFormats.Bgr32.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgr32, null, pixelData, stride);
        }

        #endregion

        #region Body

        private void DrawJoint(Joint joint, double radius, SolidColorBrush fill, double borderWidth, SolidColorBrush border)
        {
            //if (joint.TrackingState != TrackingState.Tracked) return;

            Point spacePoint1 = new Point();
            if (Visualization == Visualization.Color)
            {
                //Map the CameraPoint to ColorSpace so they match //Mapeia do "CameraPoint" para o "ColorSpace"
                ColorSpacePoint colorPoint = _coordinateMapper.MapCameraPointToColorSpace(joint.Position);

                spacePoint1.X = colorPoint.X;
                spacePoint1.Y = colorPoint.Y;

            }
            else if (Visualization == Visualization.Depth || Visualization == Visualization.Infrared)
            {
                //Map the CameraPoint to ColorSpace so they match //Mapeia do "CameraPoint" para o "ColorSpace"
                DepthSpacePoint depthPoint = _coordinateMapper.MapCameraPointToDepthSpace(joint.Position);

                spacePoint1.X = depthPoint.X;
                spacePoint1.Y = depthPoint.Y;


            }
            //Create the UI element based on the parameters //Cria os elementos do Canvas baseado nos parâmetros
            Ellipse el = new Ellipse();

            el.Fill = fill;
            el.Stroke = border;
            el.StrokeThickness = borderWidth;
            el.Width = el.Height = radius;

            //Add the Ellipse to the canvas //Adiciona a "Ellipse" no canvas
            Skeleton.Children.Add(el);

            //Avoid exceptions based on bad tracking //Evita exceções com base no mal rastreamento
            if (float.IsInfinity((float)(spacePoint1.X)) || float.IsInfinity((float)(spacePoint1.Y))) return;

            //Alinha a "Ellipse" no canvas
            Canvas.SetLeft(el, (spacePoint1.X - el.Width / 2));
            Canvas.SetTop(el, (spacePoint1.Y - el.Height / 2));
        }

        private void DrawJoint(Joint joint)
        {
            DrawJoint(joint, 5, Brushes.Blue, 0, Brushes.White);
        }

        private void DrawLine(Joint joint1, Joint joint2, double borderWidth, SolidColorBrush border)
        {
            if ((joint1.TrackingState != TrackingState.Tracked) || (joint2.TrackingState != TrackingState.Tracked)) return;

            Point spacePoint1 = new Point();
            Point spacePoint2 = new Point();
            if (Visualization == Visualization.Color)
            {
                //Map the CameraPoint to ColorSpace so they match //Mapeia do "CameraPoint" para o "ColorSpace"
                ColorSpacePoint colorPoint1 = _coordinateMapper.MapCameraPointToColorSpace(joint1.Position);
                ColorSpacePoint colorPoint2 = _coordinateMapper.MapCameraPointToColorSpace(joint2.Position);

                spacePoint1.X = colorPoint1.X;
                spacePoint1.Y = colorPoint1.Y;

                spacePoint2.X = colorPoint2.X;
                spacePoint2.Y = colorPoint2.Y;
            }
            else if (Visualization == Visualization.Depth || Visualization == Visualization.Infrared)
            {
                //Map the CameraPoint to ColorSpace so they match //Mapeia do "CameraPoint" para o "DepthSpace"
                DepthSpacePoint depthPoint1 = _coordinateMapper.MapCameraPointToDepthSpace(joint1.Position);
                DepthSpacePoint depthPoint2 = _coordinateMapper.MapCameraPointToDepthSpace(joint2.Position);

                spacePoint1.X = depthPoint1.X;
                spacePoint1.Y = depthPoint1.Y;

                spacePoint2.X = depthPoint2.X;
                spacePoint2.Y = depthPoint2.Y;
            }



            if (float.IsInfinity((float)(spacePoint1.X)) || float.IsInfinity((float)(spacePoint1.Y)) || float.IsInfinity((float)(spacePoint2.X)) || float.IsInfinity((float)(spacePoint2.Y))) return;

            //Create the UI element based on the parameters //Cria os elementos do Canvas baseado nos parâmetros
            Line line = new Line();
            line.Stroke = border;
            line.StrokeThickness = borderWidth;
            line.X1 = spacePoint1.X;
            line.Y1 = spacePoint1.Y;
            line.X2 = spacePoint2.X;
            line.Y2 = spacePoint2.Y;

            //Add the Ellipse to the canvas //Adiciona a "Ellipse" no canvas
            Skeleton.Children.Add(line);
        }

        //private void DrawLine(Point3D p1, Point3D p2, double borderWidth, SolidColorBrush border)
        //{
        //    DrawLine(MathKinect.ToJoint(p1), MathKinect.ToJoint(p2), borderWidth, border);
        //}

        private void DrawBody(Body body)
        {
            updateJoint(body);
            DrawBodyBones(body);
            DrawBodyPoints(body);

        }

        private void DrawBodyBones(Body body)
        {
            double borderWidth = 4;
            //Torso
            DrawLine(Head, Neck, borderWidth, Brushes.Red);
            DrawLine(Neck, SpineShoulder, borderWidth, Brushes.Red);
            DrawLine(SpineShoulder, SpineMid, borderWidth, Brushes.Red);
            DrawLine(SpineMid, SpineBase, borderWidth, Brushes.Red);

            // Right Torso
            DrawLine(SpineShoulder, ShoulderRight, borderWidth, Brushes.Red);
            DrawLine(SpineBase, HipRight, borderWidth, Brushes.Red);

            // Right Arm
            DrawLine(ShoulderRight, ElbowRight, borderWidth, Brushes.Red);
            DrawLine(ElbowRight, WristRight, borderWidth, Brushes.Red);
            DrawLine(WristRight, body.Joints[JointType.HandRight], borderWidth, Brushes.Red);
            DrawLine(body.Joints[JointType.HandRight], body.Joints[JointType.HandTipRight], borderWidth, Brushes.Red);
            DrawLine(WristRight, body.Joints[JointType.ThumbRight], borderWidth, Brushes.Red);

            // Right Leg
            DrawLine(HipRight, KneeRight, borderWidth, Brushes.Red);
            DrawLine(KneeRight, AnkleRight, borderWidth, Brushes.Red);
            DrawLine(AnkleRight, FootRight, borderWidth, Brushes.Red);

            // Left Torso
            DrawLine(SpineShoulder, ShoulderLeft, borderWidth, Brushes.Red);
            DrawLine(SpineBase, HipLeft, borderWidth, Brushes.Red);

            // Left Arm
            DrawLine(ShoulderLeft, ElbowLeft, borderWidth, Brushes.Red);
            DrawLine(ElbowLeft, WristLeft, borderWidth, Brushes.Red);
            DrawLine(WristLeft, body.Joints[JointType.HandLeft], borderWidth, Brushes.Red);
            DrawLine(body.Joints[JointType.HandLeft], body.Joints[JointType.HandTipLeft], borderWidth, Brushes.Red);
            DrawLine(WristLeft, body.Joints[JointType.ThumbLeft], borderWidth, Brushes.Red);

            // Left Leg
            DrawLine(HipLeft, KneeLeft, borderWidth, Brushes.Red);
            DrawLine(KneeLeft, AnkleLeft, borderWidth, Brushes.Red);
            DrawLine(AnkleLeft, FootLeft, borderWidth, Brushes.Red);

            //// Center Line
            //DrawLineWithSize(SpineShoulder, SpineBase, 1.5, 2, Brushes.Blue);

            //// Line between the knees
            //DrawLineWithSize(KneeRight, KneeLeft, 1, 2, Brushes.Blue);

            //// Line between the Shouders
            //DrawLineWithSize(ShoulderRight, ShoulderLeft, 1, 2, Brushes.Blue);

            // hip Line
            //DrawLine(AuxHipRight, AuxHipLeft, 3, Brushes.GreenYellow);


        }

        private void DrawBodyPoints(Body body)
        {
            double radius = 8;
            double borderWidth = 0;
            SolidColorBrush fill = Brushes.Yellow;
            SolidColorBrush border = Brushes.Red;

            //Draw points //Desenha os pontos
            foreach (JointType type in body.Joints.Keys)
            {
                // Draw all the body joints
                switch (type)
                {
                    case JointType.Head:
                        DrawJoint(Head, radius, fill, borderWidth, border);
                        DrawJoint(Head, 55, Brushes.Yellow, 2, Brushes.White);
                        break;
                    case JointType.Neck:
                        DrawJoint(Neck, radius, fill, borderWidth, border);
                        break;
                    case JointType.SpineShoulder:
                        DrawJoint(SpineShoulder, radius, fill, borderWidth, border);
                        break;
                    case JointType.ShoulderLeft:
                        DrawJoint(ShoulderLeft, radius, fill, borderWidth, border);
                        break;
                    case JointType.ShoulderRight:
                        DrawJoint(ShoulderRight, radius, fill, borderWidth, border);
                        break;
                    case JointType.ElbowLeft:
                        DrawJoint(ElbowLeft, radius, fill, borderWidth, border);
                        break;
                    case JointType.ElbowRight:
                        DrawJoint(ElbowRight, radius, fill, borderWidth, border);
                        break;
                    case JointType.WristLeft:
                        DrawJoint(WristLeft, radius, fill, borderWidth, border);
                        break;
                    case JointType.WristRight:
                        DrawJoint(WristRight, radius, fill, borderWidth, border);
                        break;
                    case JointType.SpineMid:
                        DrawJoint(SpineMid, radius, fill, borderWidth, border);
                        break;
                    case JointType.SpineBase:
                        DrawJoint(SpineBase, radius, fill, borderWidth, border);
                        break;
                    case JointType.HipLeft:
                        DrawJoint(HipLeft, radius, fill, borderWidth, border);
                        break;
                    case JointType.HipRight:
                        DrawJoint(HipRight, radius, fill, borderWidth, border);
                        break;
                    case JointType.KneeLeft:
                        DrawJoint(KneeLeft, radius, fill, borderWidth, border);
                        break;
                    case JointType.KneeRight:
                        DrawJoint(KneeRight, radius, fill, borderWidth, border);
                        break;
                    case JointType.AnkleLeft:
                        DrawJoint(AnkleLeft, radius, fill, borderWidth, border);
                        break;
                    case JointType.AnkleRight:
                        DrawJoint(AnkleRight, radius, fill, borderWidth, border);
                        break;
                    case JointType.FootLeft:
                        DrawJoint(FootLeft, radius, fill, borderWidth, border);

                        break;
                    case JointType.FootRight:
                        DrawJoint(FootRight, radius, fill, borderWidth, border);
                        break;

                }

            }

        }

        private void updateJoint(Body body)
        {
            FootRight = body.Joints[JointType.FootRight];
            FootLeft = body.Joints[JointType.FootLeft];
            AnkleRight = body.Joints[JointType.AnkleRight];
            AnkleLeft = body.Joints[JointType.AnkleLeft];
            KneeRight = body.Joints[JointType.KneeRight];
            KneeLeft = body.Joints[JointType.KneeLeft];
            HipRight = body.Joints[JointType.HipRight];
            HipLeft = body.Joints[JointType.HipLeft];
            SpineBase = body.Joints[JointType.SpineBase];
            SpineMid = body.Joints[JointType.SpineMid];
            WristRight = body.Joints[JointType.WristRight];
            WristLeft = body.Joints[JointType.WristLeft];
            ElbowRight = body.Joints[JointType.ElbowRight];
            ElbowLeft = body.Joints[JointType.ElbowLeft];
            ShoulderRight = body.Joints[JointType.ShoulderRight];
            ShoulderLeft = body.Joints[JointType.ShoulderLeft];
            Head = body.Joints[JointType.Head];
            Neck = body.Joints[JointType.Neck];
            SpineShoulder = body.Joints[JointType.SpineShoulder];

        }

        #endregion

        #region Properties

        public Canvas SkeletonCanvas
        {
            get { return Skeleton; }
            set { Skeleton = value; }
        }

        public Body BodyTracked
        {
            get { return bodyTracked; }
            set { }
        }

        public KStudioPlayback Playback
        {
            get { return _playback; }
            set { }
        }

        public Visualization Visualization
        {
            get { return (Visualization)GetValue(VisualizationProperty); }
            set { SetValue(VisualizationProperty, value); }
        }

        public CoordinateMapper CoordinateMapper
        {
            get { return _coordinateMapper; }
            set { }
        }

        public static readonly DependencyProperty VisualizationProperty =
            DependencyProperty.Register("Visualization", typeof(Visualization), typeof(KinectViewer), new FrameworkPropertyMetadata(Visualization.Color));

        #endregion

        #region Playback

        public void startPlayback(string filePath)
        {
            uint loopCount = 100;

            new System.Threading.Thread(() =>
            {
                using (_client = KStudio.CreateClient())
                {
                    _client.ConnectToService();
                    using (_playback = _client.CreatePlayback(filePath))
                    {
                        _playback.LoopCount = loopCount;
                        _playback.Start();
                        //_playback.Pause();

                        while (_playback.State == KStudioPlaybackState.Playing || _playback.State == KStudioPlaybackState.Paused)
                        {
                            System.Threading.Thread.Sleep(500);
                        }
                        //_playback.Stop();
                        _playback.Dispose();
                        _playback = null;
                    }
                    _client.DisconnectFromService();
                }
            }).Start();
        }

        public void playPause()
        {
            if (Playback == null) return;
            if (_playback.State == KStudioPlaybackState.Playing)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }

        #endregion

        #region Public Methods

        public void startKinect()
        {
            InitializeKinect();
            InitializeReader();
        }

        public void stopKinect()
        {
            if (_playback != null)
            {
                Pause();
                _playback.Stop();
            }
        }

        public void Clear()
        {
            Camera.Source = null;
            Skeleton.Children.Clear();
        }

        public bool IsPlaying()
        {
            if (Playback != null)
            {
                if (_playback.State == KStudioPlaybackState.Playing)
                {
                    return true;
                }

            }
            return false;
        }

        public String timePlayback()
        {
            if (Playback == null) return "";

            return _playback.CurrentRelativeTime.Minutes.ToString("00") + ":" + _playback.CurrentRelativeTime.Seconds.ToString("00") + ":" + ((_playback.CurrentRelativeTime.Milliseconds) / 10).ToString("00"); ;
        }

        public void SetTimePlayback(TimeSpan time)
        {
            if (Playback == null) return;

            TimeSpan playbackTime = _playback.CurrentRelativeTime;

            if (Convert.ToInt32(playbackTime.TotalSeconds) != Convert.ToInt32(time.TotalSeconds))
            {
                Pause();
                _playback.SeekByRelativeTime(time);
            }
        }

        public void Play()
        {
            if (_playback == null) return;
            if (_playback.State == KStudioPlaybackState.Paused) _playback.Resume();

        }

        public void Pause()
        {
            if (Playback == null) return;
            if (_playback.State == KStudioPlaybackState.Playing) _playback.Pause();

        }

        public void captureFrame(String filePath)
        {
            if (Playback == null) return;
            Pause();
            captureFrame(_depthData, (BitmapSource)(Camera.Source), filePath + "depthFrame_" + _playback.CurrentRelativeTime.Minutes + "-" +
                                                                 _playback.CurrentRelativeTime.Seconds + "-" +
                                                                 _playback.CurrentRelativeTime.Milliseconds);
            MessageBox.Show("Frame Capturado com sucesso");
        }

        public void captureSequenceFrame(String filePath)
        {
            if (Playback == null) return;
            countFrames++;
            filePathSequanceFrames = filePath;
            //MessageBox.Show("Captura de sequencia de fremes iniciada");
            Play();
        }

        public ColorSpacePoint ToColorPoint(Joint joint)
        {
            return _coordinateMapper.MapCameraPointToColorSpace(joint.Position);
        }


        #endregion

        #region Private Methods

        private void captureFrame(ushort[] data, BitmapSource Image, string fileName)
        {

            if (File.Exists(fileName + ".png"))
            {
                int i;
                for (i = 1; File.Exists(fileName + "(" + i + ")" + ".png"); ++i) ;
                fileName += "(" + i + ")";
            }

            using (System.Drawing.Image image = convertControlsImageToDrawingImage(Image))
            {


                image.Save(fileName + ".png", System.Drawing.Imaging.ImageFormat.Jpeg);
            }



            FileInfo file = new FileInfo(fileName + ".txt");



            StreamWriter escrever = file.CreateText();
            ushort[,] depth = new ushort[424, 512];

            for (int i = 0; i < data.Length; ++i)
            {
                if (i > 0 && i % 512 == 0) escrever.Write("\n");
                depth[i / 512, i - 512 * (i / 512)] = data[i];
                escrever.Write(" " + data[i]);
            }
            escrever.Write("\n");
            escrever.Close();

        }

        private System.Drawing.Image convertControlsImageToDrawingImage(BitmapSource imageControl)
        {

            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(imageControl));

            Stream ms = new MemoryStream();
            png.Save(ms);

            ms.Position = 0;

            System.Drawing.Image retImg = System.Drawing.Image.FromStream(ms);
            return retImg;

        }

        protected virtual void OnChangesMethods(EventArgs e)
        {
            EventHandler handler = ChangesMethods;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }
}
