using System;
using System.Diagnostics;
using Microsoft.Kinect;
using System.Text;
using System.Windows;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Kinect.Tools;

namespace CaptureBody
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Body body = null;
        double height = 0;
        double left = 0;
        double right = 0;
        double rightHipAngle = 0;
        double shoulderFlexion = 0;
        double neckFlexion = 0;
        double neckExtension = 0;

        public MainWindow()
        {
            InitializeComponent();
            viewer.ChangesMethods += viewer_ChangesMethods;
            viewer.startKinect();
        }

        void AngleVariables(Body body)
        {
            // Calculate angles.
            height = Math.Round(body.UpperHeight(), 2);
            left = Math.Round(body.LeftHand(), 2);
            right = Math.Round(body.RightHand(), 2);
            rightHipAngle = Math.Round(body.HipRelativeAngle(), 2);
            shoulderFlexion = Math.Round(body.ShoulderFlexion(), 2);
            neckFlexion = Math.Round(body.neckFlexion(), 2);
            neckExtension = Math.Round(body.neckExtension(), 2);
        }

        void Sensor_SkeletonFrameReady()
        {

            AngleVariables(body);

            // Display height.
            tblHeight.Text = "Height: " + height.ToString() + "m";

            //Display Arms
            tblLeft.Text = "Left: " + left.ToString() + "m";
            tblRight.Text = "Right: " + right.ToString() + "m";
            //tblAngleLeft.Text = "Relative Angle Left: " + leftArmRelativeAngle.ToString() + "º";
            tblAngleRight.Text = "Angle Hip Right: " + rightHipAngle.ToString() + "º";
            tblShoulderFlexion.Text = "Shoulder Flexion: " + shoulderFlexion.ToString() + "º";
            tblNeckFlexion.Text = "Neck Flexion: " + neckFlexion.ToString() + "º";
            tblNeckExtension.Text = "Neck Extension: " + neckExtension.ToString() + "º";

            //Display bodyPositions
            //Display LeftArmPositions
            tblPositionHeaderX.Text = "Position Head X: " + body.Joints[JointType.Head].Position.X;
            tblPositionHeaderY.Text = "Position Head Y: " + body.Joints[JointType.Head].Position.Y;
            tblPositionHeaderZ.Text = "Position Head Z: " + body.Joints[JointType.Head].Position.Z;
            tblPositionShoulderRightX.Text = "Position Right Shoulder Y: " + body.Joints[JointType.ShoulderRight].Position.Y;
            tblPositionShoulderRightY.Text = "Position Right Shoulder X: " + body.Joints[JointType.ShoulderRight].Position.X;
            tblPositionShoulderRightZ.Text = "Position Right Shoulder Y: " + body.Joints[JointType.ShoulderRight].Position.Y;
            tblPositionElbowRightX.Text = "Position Rigth Elbow Y: " + body.Joints[JointType.ElbowRight].Position.Y;
            tblPositionElbowRightY.Text = "Position Rigth Elbow X: " + body.Joints[JointType.ElbowRight].Position.X;
            tblPositionElbowRightZ.Text = "Position Rigth Elbow Y: " + body.Joints[JointType.ElbowRight].Position.Y;
      
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

        private void AngleString(StringBuilder csvContent, Body body)
        {
            string aux;
            aux = "Height," + height.ToString() + "m";
            csvContent.AppendLine(aux);
            // Display height.

            //Display Arms
            aux = "Left," + left.ToString() + "m";
            csvContent.AppendLine(aux);
            aux = "Right," + right.ToString() + "m";
            csvContent.AppendLine(aux);

            //tblAngleLeft.Text = "Relative Angle Left: " + leftArmRelativeAngle.ToString() + "º";
            aux = "Angle Hip Right," + rightHipAngle.ToString() + "º";
            csvContent.AppendLine(aux);
            aux = "Shoulder Flexion," + shoulderFlexion.ToString() + "º";
            csvContent.AppendLine(aux);
            aux = "Neck Flexion," + neckFlexion.ToString() + "º";
            csvContent.AppendLine(aux);
            aux = "Neck Extension," + neckExtension.ToString() + "º";
            csvContent.AppendLine(aux);

            //Display bodyPositions
            //Display LeftArmPositions
            aux = "Position Head X," + body.Joints[JointType.Head].Position.X;
            csvContent.AppendLine(aux);
            aux = "Position Head Y," + body.Joints[JointType.Head].Position.Y;
            csvContent.AppendLine(aux);
            aux = "Position Head Z: " + body.Joints[JointType.Head].Position.Z;
            csvContent.AppendLine(aux);
            aux = "Position Right Shoulder Y," + body.Joints[JointType.ShoulderRight].Position.Y;
            csvContent.AppendLine(aux);
            aux = "Position Right Shoulder X," + body.Joints[JointType.ShoulderRight].Position.X;
            csvContent.AppendLine(aux);
            aux = "Position Right Shoulder Y," + body.Joints[JointType.ShoulderRight].Position.Y;
            csvContent.AppendLine(aux);
            aux = "Position Rigth Elbow Y," + body.Joints[JointType.ElbowRight].Position.Y;
            csvContent.AppendLine(aux);
            aux = "Position Rigth Elbow X," + body.Joints[JointType.ElbowRight].Position.X;
            csvContent.AppendLine(aux);
            aux = "Position Rigth Elbow Y," + body.Joints[JointType.ElbowRight].Position.Y;
            csvContent.AppendLine(aux);
        }

        private void ScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("Type,Angle");
            AngleString(csvContent, body);
            Debug.WriteLine(csvContent);
            string time = System.DateTime.UtcNow.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);

            string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            string path = Path.Combine(myPhotos, "KinectScreenshot-BodyIndex-" + time + ".png");

            /*if (this.bodyIndexBitmap != null)
            {
                // create a png bitmap encoder which knows how to save a .png file
                BitmapEncoder encoder = new PngBitmapEncoder();

                // create frame from the writable bitmap and add to encoder
                encoder.Frames.Add(BitmapFrame.Create(this.bodyIndexBitmap));

                string time = System.DateTime.UtcNow.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);

                string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                string path = Path.Combine(myPhotos, "KinectScreenshot-BodyIndex-" + time + ".png");

                // write the new file to disk
                try
                {
                    // FileStream is IDisposable
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }

                    this.StatusText = string.Format(CultureInfo.CurrentCulture, Properties.Resources.SavedScreenshotStatusTextFormat, path);
                }
                catch (IOException)
                {
                    this.StatusText = string.Format(CultureInfo.CurrentCulture, Properties.Resources.FailedScreenshotStatusTextFormat, path);
                }
            }*/

        }




        private void Play_Click(object sender, RoutedEventArgs e)
        {
            viewer.playPause();
        }



    }


}