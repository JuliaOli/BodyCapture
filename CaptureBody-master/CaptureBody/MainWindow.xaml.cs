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
        double height = 0;
        double left = 0;
        double right = 0;
        double rightHipAngle = 0;

        //Trunk
        double trunk = 0;

        //neck flexion
        double neckFlexion = 0;
        double neckExtension = 0;
        
        //abducao de ombro
        double rightShoulderAbduction = 0;
        double leftShoulderAbduction = 0;

        //flexao de ombro
        double rightShoulderFlexion = 0;
        double leftShoulderFlexion = 0;

        //Timer
        private int incrementSeconds = 0;
        private int incrementMinuts = 0;
        //Timer Checker
        bool incrementTrunk = false;
        bool incrementHip = false;
        bool incrementFlex = false;
        bool incrementAbd = false;
        bool incrementNeckFlex = false;
        bool incrementNeckExt = false;

        public MainWindow()
        {
            InitializeComponent();
            viewer.ChangesMethods += viewer_ChangesMethods;
            viewer.startKinect();
            Timer();
        }

        void AngleVariables(Body body)
        {
            // Calculate angles.
            height = Math.Round(body.UpperHeight(), 2);
            left = Math.Round(body.LeftHand(), 2);
            right = Math.Round(body.RightHand(), 2);
            rightHipAngle = Math.Round(body.HipRelativeAngle(), 2);
            trunk = Math.Round(body.TrunkFlexion(), 2);

            //Flexao de ombro
            rightShoulderFlexion = Math.Round(body.RightShoulderFlexion(), 2);
            leftShoulderFlexion = Math.Round(body.LeftShoulderFlexion(), 2);

            //Neck
            neckFlexion = Math.Round(body.neckFlexion(), 2);
            neckExtension = Math.Round(body.neckExtension(), 2);

            //Abducao de ombro
            rightShoulderAbduction = Math.Round(body.RightShoulderAbduction(), 2);
            leftShoulderAbduction = Math.Round(body.LeftShoulderAbduction(), 2);
            
            Debug.WriteLine(leftShoulderFlexion);
        }

        void Sensor_SkeletonFrameReady()
        {

            AngleVariables(body);
            anglesrules = new AngleRules(body);
            Color colorAux1;
            Color colorAux2;

            // Display height.
            tblHeight.Text = "Height: " + height.ToString() + "m";

            //Display Arms
            tblLeft.Text = "Left: " + left.ToString() + "m";
            tblRight.Text = "Right: " + right.ToString() + "m";
            //tblAngleLeft.Text = "Relative Angle Left: " + leftArmRelativeAngle.ToString() + "º";

            //Hip Angle
            colorAux1 = anglesrules.hipRisk(rightHipAngle);
            colorAux2 = anglesrules.hipRisk(rightHipAngle);
            tblAngleRight.Text = "Angle Hip Right: " + rightHipAngle.ToString() + "º";
            tblAngleRight.Foreground = new SolidColorBrush(colorAux1);
            incrementHip = anglesrules.colorCheck(colorAux1,
               colorAux1);

            //Trunk

            colorAux1 = anglesrules.trunkRisk(trunk);
            colorAux2 = anglesrules.trunkRisk(trunk);
            tblTrunk.Text = "Anterior trunk inclination" + trunk.ToString() + "º";
            tblTrunk.Foreground = new SolidColorBrush(colorAux1);
            incrementTrunk = anglesrules.colorCheck(colorAux1,
                colorAux1);


            //Flexion
            tblRightShoulderFlexion.Text = "Right Shoulder Flexion: " + rightShoulderFlexion.ToString() + "º";
            tblLeftShoulderFlexion.Text = "Left Shoulder Flexion: " + leftShoulderFlexion.ToString() + "º";
            colorAux1 = anglesrules.getRiskColor(rightShoulderFlexion, neckFlexion, neckExtension);
            colorAux2 = anglesrules.getRiskColor(leftShoulderFlexion, neckFlexion, neckExtension);
            incrementFlex = anglesrules.colorCheck(anglesrules.getRiskColor(rightShoulderFlexion, neckFlexion, neckExtension), 
                anglesrules.getRiskColor(rightShoulderFlexion, neckFlexion, neckExtension));
            tblRightShoulderFlexion.Foreground = new SolidColorBrush(colorAux1);
            tblLeftShoulderFlexion.Foreground = new SolidColorBrush(colorAux2);

            //abduction
            tblRightShoulderAbduction.Text = "Right Shoulder Abduction: " + rightShoulderAbduction.ToString() + "º";
            tblLeftShoulderAbduction.Text = "Left Shoulder Abduction: " + leftShoulderAbduction.ToString() + "º";
            colorAux1 = anglesrules.abductionRisk(leftShoulderAbduction);
            colorAux2 = anglesrules.abductionRisk(rightShoulderAbduction);
            incrementAbd = anglesrules.colorCheck(anglesrules.abductionRisk(leftShoulderAbduction),
                anglesrules.abductionRisk(rightShoulderAbduction));
            tblLeftShoulderAbduction.Foreground = new SolidColorBrush(colorAux1);
            tblRightShoulderAbduction.Foreground = new SolidColorBrush(colorAux2);

            //Neck 
            tblNeckFlexion.Text = "Neck Flexion: " + neckFlexion.ToString() + "º";
            tblNeckExtension.Text = "Neck Extension: " + neckExtension.ToString() + "º";
            colorAux1 = anglesrules.getRiskColor(rightShoulderFlexion, neckFlexion, neckExtension);
            colorAux2 = anglesrules.getRiskColor(rightShoulderFlexion, neckFlexion, neckExtension);
            //Só vou confiar que está certo
            incrementNeckFlex = anglesrules.colorCheck(colorAux1, colorAux1);
            incrementNeckExt = anglesrules.colorCheck(colorAux2, colorAux2);
            tblNeckFlexion.Foreground = new SolidColorBrush(anglesrules.getRiskColor(rightShoulderFlexion, neckFlexion, neckExtension));
            tblNeckExtension.Foreground = new SolidColorBrush(anglesrules.getRiskColor(leftShoulderFlexion, neckFlexion, neckExtension));

            //Display bodyPositions
            //Display Head positions
            tblPositionHeaderX.Text = "Position Head X: " + body.Joints[JointType.Head].Position.X;
            tblPositionHeaderY.Text = "Position Head Y: " + body.Joints[JointType.Head].Position.Y;
            tblPositionHeaderZ.Text = "Position Head Z: " + body.Joints[JointType.Head].Position.Z;

            // Display Left Shoulder Positions
            tblPositionShoulderLeftX.Text = "Position Shoulder Left X: " + body.Joints[JointType.ShoulderLeft].Position.X;
            tblPositionShoulderLeftY.Text = "Position Shoulder Left Y: " + body.Joints[JointType.ShoulderLeft].Position.Y;
            tblPositionShoulderLeftZ.Text = "Position Shoulder Left Z: " + body.Joints[JointType.ShoulderLeft].Position.Z;
            tblPositionElbowLeftX.Text = "Position Left Elbow X: " + body.Joints[JointType.ElbowRight].Position.X;
            tblPositionElbowLeftY.Text = "Position Left Elbow Y: " + body.Joints[JointType.ElbowRight].Position.Y;
            tblPositionElbowLeftZ.Text = "Position Left Elbow Z: " + body.Joints[JointType.ElbowRight].Position.Z;

            //Display Right Shoulder Positions
            tblPositionShoulderRightX.Text = "Position Right Shoulder X: " + body.Joints[JointType.ShoulderRight].Position.X;
            tblPositionShoulderRightY.Text = "Position Right Shoulder Y: " + body.Joints[JointType.ShoulderRight].Position.Y;
            tblPositionShoulderRightZ.Text = "Position Right Shoulder Z: " + body.Joints[JointType.ShoulderRight].Position.Z;
            tblPositionElbowRightX.Text = "Position Rigth Elbow Y: " + body.Joints[JointType.ElbowRight].Position.X;
            tblPositionElbowRightY.Text = "Position Rigth Elbow X: " + body.Joints[JointType.ElbowRight].Position.Y;
            tblPositionElbowRightZ.Text = "Position Rigth Elbow Y: " + body.Joints[JointType.ElbowRight].Position.Z;
            
        }

        private void viewer_ChangesMethods(object sender, EventArgs e)
        {

            UpdateBody();
        }

        private void UpdateBody()
        {
            //if (viewer.Playback == null) return;
            this.body = viewer.BodyTracked;
            if (body == null) return;
            Sensor_SkeletonFrameReady();
            

        }
        /*<Button x:Name="btnRecord" Content="Record" HorizontalAlignment="Left" Margin="0,10" VerticalAlignment="Top" Width="200" Height="20" ToolTip="Gravar" Click="btnRecord_Click"/>
            <Button x:Name="btnStop" Content="Stop" HorizontalAlignment="Left" Margin="0,10" VerticalAlignment="Top" Width="200" Height="20" ToolTip="Gravar" IsEnabled="False"  Click="btnStop_Click"/>
        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            btnStop.IsEnabled = true;
            Sensor_SkeletonFrameReady();
        }
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            btnRecord.Focus();
            btnStop.IsEnabled = false;
        }*/

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
        
        //FUNCTION DIRECT TO NEW CLASSE (CONSTRUCTOR RECIEVE VARIABLE BODY) 
        /*
        private void AngleString(StringBuilder csvContent, Body body)
        {
            string aux;
            aux = "Height;" + height.ToString() + "m";
            csvContent.AppendLine(aux);
            // Display height.

            //Display Arms
            aux = "Left;" + left.ToString() + "m";
            csvContent.AppendLine(aux);
            aux = "Right;" + right.ToString() + "m";
            csvContent.AppendLine(aux);

            //tblAngleLeft.Text = "Relative Angle Left: " + leftArmRelativeAngle.ToString() + "º";
            aux = "Angle Hip Right;" + rightHipAngle.ToString() + " graus";
            csvContent.AppendLine(aux);
            
            //flexion
            aux = "Right Shoulder Flexion;" + rightShoulderFlexion.ToString() + " graus";
            csvContent.AppendLine(aux);
            aux = "Left Shoulder Flexion;" + leftShoulderFlexion.ToString() + " graus";
            csvContent.AppendLine(aux);
            
            //abduction
            aux = "Right Shoulder Abduction;" + rightShoulderAbduction.ToString() + " graus";
            csvContent.AppendLine(aux);
            aux = "Left Shoulder Abduction;" + leftShoulderAbduction.ToString() + " graus";
            csvContent.AppendLine(aux);

            //Neck
            aux = "Neck Flexion;" + neckFlexion.ToString() + " graus";
            csvContent.AppendLine(aux);
            aux = "Neck Extension;" + neckExtension.ToString() + " graus";
            csvContent.AppendLine(aux);

            //Display bodyPositions
            //Display LeftArmPositions
            try
            {
                // FileStream is IDisposable
                aux = "Position Head X;" + body.Joints[JointType.Head].Position.X.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Head Y;" + body.Joints[JointType.Head].Position.Y.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Head Z," + body.Joints[JointType.Head].Position.Z.ToString();
                csvContent.AppendLine(aux);

                //Left Shoulder and Elbow Positions
                aux = "Position Left Shoulder X;" + body.Joints[JointType.ShoulderLeft].Position.X.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Left Shoulder Y;" + body.Joints[JointType.ShoulderLeft].Position.Y.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Left Shoulder Z;" + body.Joints[JointType.ShoulderLeft].Position.Z.ToString();
                csvContent.AppendLine(aux).ToString();
                aux = "Position Left Elbow X;" + body.Joints[JointType.ElbowLeft].Position.X.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Left Elbow Y;" + body.Joints[JointType.ElbowLeft].Position.Y.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Left Elbow Z;" + body.Joints[JointType.ElbowLeft].Position.Z.ToString();
                csvContent.AppendLine(aux);
                
                //Right Shoulder and Elbow Positions
                aux = "Position Right Shoulder Y;" + body.Joints[JointType.ShoulderRight].Position.X.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Right Shoulder X;" + body.Joints[JointType.ShoulderRight].Position.Y.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Right Shoulder Y;" + body.Joints[JointType.ShoulderRight].Position.Z.ToString();
                csvContent.AppendLine(aux).ToString();
                aux = "Position Rigth Elbow Y;" + body.Joints[JointType.ElbowRight].Position.X.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Rigth Elbow X;" + body.Joints[JointType.ElbowRight].Position.Y.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Rigth Elbow Y;" + body.Joints[JointType.ElbowRight].Position.Z.ToString();

                csvContent.AppendLine(aux);
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Erro loading the file. " + "Variable body hasn't been set yet");
            }
            
        }*/

        private void Timer()
        {

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTicker;
            dt.Start();
        }

        private void dtTicker(object sender, EventArgs e)
        {
            ++incrementSeconds;
            
            if(incrementSeconds == 60)
            {
                incrementSeconds = 0;
                ++incrementMinuts;
            }
            tblTimer.Content = incrementMinuts.ToString() + ":" + incrementSeconds.ToString();

        }


        private void ScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            anglesrules.CsvBuilder(body, viewer);





            /*
            StringBuilder csvContent = new StringBuilder();
            if(body != null && viewer.Camera != null)
            {
                csvContent.AppendLine("Type;Angle/Lenght/Position");
                AngleString(csvContent, body);
                
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

                //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                
                //Create a new folder
                string folderName = "KinectSaveBodyInformation-BodyIndex-" + time;
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
                catch (IOException)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
                finally { }
                

        
                //Name of the files (csv,png,png(canvas))
                string path = Path.Combine(subFolder, "KinectSaveBodyInformation-" + time + ".csv");

                //Debug.WriteLine(subFolder);
                
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
            }*/

        }




        private void Play_Click(object sender, RoutedEventArgs e)
        {
            viewer.playPause();
        }



    }

}