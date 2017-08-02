using System;
using Microsoft.Kinect;
using System.Windows;
using System.Windows.Forms;

namespace CaptureBody
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Body body = null;

        public MainWindow()
        {
            InitializeComponent();
            viewer.ChangesMethods += viewer_ChangesMethods;
            viewer.startKinect();
        }

        void Sensor_SkeletonFrameReady()
        {
            // Calculate height.
            double height = Math.Round(body.UpperHeight(), 2);
            double left = Math.Round(body.LeftHand(), 2);
            double right = Math.Round(body.RightHand(), 2);
            double rightHipAngle = Math.Round(body.HipRelativeAngle(), 2);
            double shoulderFlexion = Math.Round(body.ShoulderFlexion(), 2);
            double neckFlexion = Math.Round(body.neckFlexion(), 2);
            double neckExtension = Math.Round(body.neckExtension(), 2);
            

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

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            viewer.playPause();
        }



    }

}
