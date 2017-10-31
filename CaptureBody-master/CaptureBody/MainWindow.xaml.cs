using System;
using System.Diagnostics;
using Microsoft.Kinect;
using System.Text;
using System.Windows;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Kinect.Tools;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;

namespace CaptureBody
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AngleRules anglesrules;
        private Body body = null;
        
        //Timer
        Timer rightHipTimer = new Timer();
        Timer leftHipTimer = new Timer();
        Timer trunkTimer = new Timer();
        Timer rightShoulderFlexionTimer = new Timer();
        Timer leftShoulderFlexionTimer = new Timer();
        Timer rightShoulderAbductionTimer = new Timer();
        Timer leftShoulderAbductionTimer = new Timer();
        Timer neckFlexionTimer = new Timer();

        //Timer Checker
        private bool recorrenceLeftHip = false;
        private bool recorrenceRightHip = false;
        private bool recorrenceRightShoulderFlexion = false;
        private bool recorrenceLeftShoulderFlexion = false;
        private bool recorrenceRightShoulderAbduction = false;
        private bool recorrenceLeftShoulderAbduction = false;
        private bool recorrenceTrunk = false;
        private bool recorrenceNeck = false; 
        
        public MainWindow()
        {
            InitializeComponent();
            viewer.ChangesMethods += viewer_ChangesMethods;
            viewer.startKinect();
        }
        
        void Sensor_SkeletonFrameReady()
        {
            anglesrules = new AngleRules(body);
            Color colorAux1;

            try
            {
                Checkboxes();
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Variables wasn't atributed");
            }

            // Display height.
            tblHeight.Text = "Height: " + anglesrules.Height.ToString() + "m";
            
            //Display Head positions
            tblPositionHeaderX.Text = "Position Head X: " + body.Joints[JointType.Head].Position.X;
            tblPositionHeaderY.Text = "Position Head Y: " + body.Joints[JointType.Head].Position.Y;
            tblPositionHeaderZ.Text = "Position Head Z: " + body.Joints[JointType.Head].Position.Z;

            //Display Arms
            tblLeft.Text = "Left: " + anglesrules.Left.ToString() + "m";
            tblRight.Text = "Right: " + anglesrules.Right.ToString() + "m";
            /*
            //Right Hip Flexion
            colorAux1 = anglesrules.hipRisk(anglesrules.RightHipFlexion);
            tblRightHipFlexion.Foreground = new SolidColorBrush(colorAux1);

            if (anglesrules.colorCheck(colorAux1))
            {
                if (recorrenceRightHip == false)
                {
                    rightHipTimer.StartTimer();
                    recorrenceRightHip = true;
                }
            }
            else
            {
                recorrenceRightHip = false;
                rightHipTimer.ResetTimer();
            }
            tblRightHipFlexion.Text = "Right Hip Angle: " + anglesrules.RightHipFlexion.ToString() + "°  " + rightHipTimer.TimerVar;

            //Left Hip Flexion
            colorAux1 = anglesrules.hipRisk(anglesrules.LeftHipFlexion);
            tblLeftHipFlexion.Foreground = new SolidColorBrush(colorAux1);

            if (anglesrules.colorCheck(colorAux1))
            {
                if (recorrenceLeftHip == false)
                {
                    leftHipTimer.StartTimer();
                    recorrenceLeftHip = true;
                }
            }
            else
            {
                recorrenceLeftHip = false;
                leftHipTimer.ResetTimer();
            }
            tblLeftHipFlexion.Text = "Left Hip Angle: " + anglesrules.LeftHipFlexion.ToString() + "° " + leftHipTimer.TimerVar;
            */
            //Trunk
            colorAux1 = anglesrules.trunkRisk(anglesrules.Trunk);
            tblTrunk.Foreground = new SolidColorBrush(colorAux1);
            
            if (anglesrules.colorCheck(colorAux1))
            {
                if (recorrenceTrunk == false)
                {
                    trunkTimer.StartTimer();
                    recorrenceTrunk = true;
                }

            }
            else
            {
                recorrenceTrunk = false;
                trunkTimer.ResetTimer();
            }
            tblTrunk.Text = "Anterior trunk inclination" + anglesrules.Trunk.ToString() + "° " + trunkTimer.TimerVar;

            //Right Shoulder Flexion
            colorAux1 = anglesrules.shoulderFlexionRisk(anglesrules.RightShoulderFlexion);
            tblRightShoulderFlexion.Foreground = new SolidColorBrush(colorAux1);
            if (anglesrules.colorCheck(colorAux1))
            {
                if (recorrenceRightShoulderFlexion == false)
                {
                    rightShoulderFlexionTimer.StartTimer();
                    recorrenceRightShoulderFlexion = true;
                }

            }
            else
            {
                recorrenceRightShoulderFlexion = false;
                rightShoulderFlexionTimer.ResetTimer();
            }
            tblRightShoulderFlexion.Text = "Right Shoulder Flexion: " + anglesrules.RightShoulderFlexion.ToString() + "° " + rightShoulderFlexionTimer.TimerVar;

            //Left Shoulder Flexion
            colorAux1 = anglesrules.shoulderFlexionRisk(anglesrules.LeftShoulderFlexion);
            tblLeftShoulderFlexion.Foreground = new SolidColorBrush(colorAux1);
            if (anglesrules.colorCheck(colorAux1))
            {
                if (recorrenceLeftShoulderFlexion == false)
                {
                    leftShoulderFlexionTimer.StartTimer();
                    recorrenceLeftShoulderFlexion = true;
                }

            }
            else
            {
                recorrenceLeftShoulderFlexion = false;
                leftShoulderFlexionTimer.ResetTimer();
            }
            tblLeftShoulderFlexion.Text = "Left Shoulder Flexion: " + anglesrules.LeftShoulderFlexion.ToString() + "° " + leftShoulderFlexionTimer.TimerVar;

            //Right Shoulder Abduction
            colorAux1 = anglesrules.shoulderAbductionRisk(anglesrules.RightShoulderAbduction);
            tblRightShoulderAbduction.Foreground = new SolidColorBrush(colorAux1);
            if (anglesrules.colorCheck(colorAux1))
            {
                if (recorrenceRightShoulderAbduction == false)
                {
                    rightShoulderAbductionTimer.StartTimer();
                    recorrenceRightShoulderAbduction = true;
                }

            }
            else
            {
                recorrenceRightShoulderAbduction = false;
                rightShoulderAbductionTimer.ResetTimer();
            }
            tblRightShoulderAbduction.Text = "Right Shoulder Abduction: " + anglesrules.RightShoulderAbduction.ToString() + "° " + rightShoulderAbductionTimer.TimerVar;

            //Left Shoulder Abduction
            colorAux1 = anglesrules.shoulderAbductionRisk(anglesrules.LeftShoulderAbduction);
            tblLeftShoulderAbduction.Foreground = new SolidColorBrush(colorAux1);
            if (anglesrules.colorCheck(colorAux1))
            {
                if (recorrenceLeftShoulderAbduction == false)
                {
                    leftShoulderAbductionTimer.StartTimer();
                    recorrenceLeftShoulderAbduction = true;
                }

            }
            else
            {
                recorrenceLeftShoulderAbduction = false;
                leftShoulderAbductionTimer.ResetTimer();
            }
            tblLeftShoulderAbduction.Text = "Left Shoulder Abduction: " + anglesrules.LeftShoulderAbduction.ToString() + "° " + leftShoulderAbductionTimer.TimerVar;

            //Neck
            colorAux1 = anglesrules.neckFlexionRisk(anglesrules.NeckFlexion);
            tblNeckFlexion.Foreground = new SolidColorBrush(colorAux1);
            if (anglesrules.colorCheck(colorAux1))
            {
                if (recorrenceNeck == false)
                {
                    neckFlexionTimer.StartTimer();
                    recorrenceNeck = true;
                }

            }
            else
            {
                recorrenceNeck = false;
                neckFlexionTimer.ResetTimer();
            }
            tblNeckFlexion.Text = "Neck Flexion: " + anglesrules.NeckFlexion.ToString() + "° " + neckFlexionTimer.TimerVar;
            
            //Neck Extension
            tblNeckExtension.Text = "Neck Extension: " + anglesrules.NeckExtension.ToString() + "°";
            
            if(neckFlexionTimer.TimerVar == "Timer: 08:30" || trunkTimer.TimerVar == "Timer: 03:00"
                || leftShoulderAbductionTimer.TimerVar == "Timer: 04:00" || rightShoulderAbductionTimer.TimerVar == "Timer: 04:00")
            {
                try
                {
                    CsvBuilder(body);
                }
                catch (NullReferenceException)
                {
                    Debug.WriteLine("Variables wasn't atributed");
                }
            }
            
        }

        private void viewer_ChangesMethods(object sender, EventArgs e)
        {
            UpdateBody();
        }

        private void UpdateBody()
        {
            this.body = viewer.BodyTracked;
            if (body == null) return;
            Sensor_SkeletonFrameReady();
        }
        
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            viewer.stopPlayback();
        }

        private void Playback_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Event files|*.xef";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                viewer.startPlayback(filePath);

            }
        }
        
        private void ScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CsvBuilder(body);
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Variables wasn't atributed");
            }
        }
        
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            viewer.playPause();
        }

        public void Checkboxes(){

            
            if ((bool)ElevationS.IsChecked == true)
            {
                anglesrules.ElevationS = true;
            }
            else
            {
                anglesrules.ElevationS = false;
            }

            if ((bool)InadequateS.IsChecked == true) 
            {
                anglesrules.InadequateS = true;
            }
            else
            {
                anglesrules.InadequateS = false;
            }

            if ((bool)SymetryN.IsChecked == true)
            {
                anglesrules.SymetryN = true;
            }
            else
            {
                anglesrules.SymetryN = false;
            }

            if ((bool)RectifinedT.IsChecked == true)
            {
                anglesrules.RectifinedT = true;
            }
            else
            {
                anglesrules.RectifinedT = false;
            }

            if ((bool)SymetryT.IsChecked == true)
            {
                anglesrules.SymetryT = true;
            }
            else
            {
                anglesrules.SymetryT = false;
            }
        }

        /// <Csv File Preparations>
        /// The Strings are treated to be in the format that is more suited 
        /// </Csv File Preparations>
        /// <param name="csvContent"></param>
        /// <param name="body"></param>

        public StringBuilder AngleString(Body body)
        {
            StringBuilder csvContent = new StringBuilder();
            string aux;

            //Shoulders csv printer
            if (anglesrules.ElevationS)
            {
                aux = "Shoulder Elevation:; Not recommended"; 
            }
            else
            {
                aux = "Shoulder Elevation:; Acceptable";
            }
            csvContent.AppendLine(aux);

            if (anglesrules.InadequateS)
            {
                aux = "Inadequate Arm Posture:; Not recommended"; 
            }
            else
            {
                aux = "Inadequate Arm Posture:; Acceptable";
            }
            csvContent.AppendLine(aux);

            aux = tblRightShoulderFlexion.Text.ToString();
            csvContent.AppendLine(aux);
            aux = tblLeftShoulderFlexion.Text.ToString();
            csvContent.AppendLine(aux);
            aux = tblRightShoulderAbduction.Text.ToString();
            csvContent.AppendLine(aux);
            aux = tblLeftShoulderAbduction.Text.ToString();
            csvContent.AppendLine(aux);
            
            //Neck
            if (anglesrules.SymetryN)
            {
                aux = "Neck Symmetry:; Not recommended"; 
            }
            else
            {
                aux = "Neck Symmetry:; Acceptable";
            }
            csvContent.AppendLine(aux);

            aux = tblNeckFlexion.Text.ToString();
            csvContent.AppendLine(aux);
            aux = tblNeckExtension.Text.ToString();
            csvContent.AppendLine(aux);

            //Trunk
            if (anglesrules.RectifinedT)
            {
                aux = "Rectified Spine:; Not recommended"; 
            }
            else
            {
                aux = "Rectified Spine:; Acceptable";
            }
            csvContent.AppendLine(aux);
            if (anglesrules.SymetryT)
            {
                aux = "Trunk Symmetry:; Not recommended"; 
            }
            else
            {
                aux = "Trunk Symmetry:; Acceptable";
            }
            csvContent.AppendLine(aux);

            aux = tblTrunk.Text.ToString();
            csvContent.AppendLine(aux);
            /*aux = tblRightHipFlexion.Text.ToString();
            csvContent.AppendLine(aux);
            aux = tblLeftHipFlexion.Text.ToString();
            csvContent.AppendLine(aux);*/

            // Display height.
            aux = tblHeight.Text.ToString();
            csvContent.AppendLine(aux);

            //Display Arms
            aux = tblLeft.Text.ToString();
            csvContent.AppendLine(aux);
            aux = tblRight.Text.ToString();
            csvContent.AppendLine(aux);
            aux = tblPositionHeaderX.Text.ToString();
            csvContent.AppendLine(aux);
            aux = tblPositionHeaderY.Text.ToString();
            csvContent.AppendLine(aux);
            aux = tblPositionHeaderZ.Text.ToString();
            csvContent.AppendLine(aux);

            return csvContent;
        }

        /// <CSVFile>
        /// Builds the file with the csv and a printscreen from the body skeleton and rgb image
        /// </summary>
        /// <param name="body"></param>
        /// <param name="viewer"></param>

        public void CsvBuilder(Body body)
        {
            StringBuilder csvContent = AngleString(body);
            if (body != null && viewer.Camera != null)
            {
                csvContent.AppendLine("Type;Angle/Lenght/Position");
                csvContent = AngleString(body);

                string time = System.DateTime.UtcNow.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);
                string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // create a png bitmap encoder which knows how to save a .png file
                BitmapEncoder encoder = new PngBitmapEncoder();
                BitmapEncoder encoderCanvas = new PngBitmapEncoder();

                // create frame from the writable bitmap and add to encoder
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)viewer.Camera.Source));

                Int32 width = (Int32)viewer.Skeleton.ActualWidth;
                Int32 height = (Int32)viewer.Skeleton.ActualHeight;

                RenderTargetBitmap renderBitmap = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Pbgra32);
                renderBitmap.Render(viewer.Skeleton);

                PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                //Create a new folder
                string folderName = "CaptureBody-Screenshot" + time;
                string subFolder = Path.Combine(myPhotos, folderName);

                try
                {
                    // Determine whether the directory exists.
                    if (Directory.Exists(subFolder))
                    {
                        Console.WriteLine("That path exists already.");
                        return;
                    }

                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(subFolder);
                    Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(subFolder));

                }
                catch (IOException e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
                finally { }

                //Name of the files (csv,png,png(canvas))
                string path = Path.Combine(subFolder, "KinectSaveBodyInformation-" + time + ".csv");


                string imagePath = Path.Combine(subFolder, "CameraImage-" + time + ".png");

                //canvas

                string imagePathCanvas = Path.Combine(subFolder, "Skeleton-" + time + ".png");

                Debug.WriteLine(imagePath);


                try
                {
                    using (FileStream fs = new FileStream(imagePath, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }

                    using (FileStream fs = new FileStream(imagePathCanvas, FileMode.Create))
                    {
                        pngEncoder.Save(fs);
                    }

                    File.AppendAllText(path, csvContent.ToString());
                }
                catch (IOException)
                {
                    Debug.WriteLine("Erro writing the file");
                }

            }

        }
        
    }

}