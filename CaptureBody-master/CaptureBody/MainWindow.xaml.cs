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
            //tblPositionHeaderY.Text = "Position Head Y: " + body.Joints[JointType.Head].Position.Y;
           //tblPositionHeaderZ.Text = "Position Head Z: " + body.Joints[JointType.Head].Position.Z;

            // Display Left Shoulder Positions
            //tblPositionShoulderLeftX.Text = "Position Shoulder Left X: " + body.Joints[JointType.ShoulderLeft].Position.X;
            //tblPositionShoulderLeftY.Text = "Position Shoulder Left Y: " + body.Joints[JointType.ShoulderLeft].Position.Y;
            //tblPositionShoulderLeftZ.Text = "Position Shoulder Left Z: " + body.Joints[JointType.ShoulderLeft].Position.Z;
            //tblPositionElbowLeftX.Text = "Position Left Elbow X: " + body.Joints[JointType.ElbowRight].Position.X;
            //tblPositionElbowLeftY.Text = "Position Left Elbow Y: " + body.Joints[JointType.ElbowRight].Position.Y;
            //tblPositionElbowLeftZ.Text = "Position Left Elbow Z: " + body.Joints[JointType.ElbowRight].Position.Z;

            //Display Right Shoulder Positions
            //tblPositionShoulderRightX.Text = "Position Right Shoulder X: " + body.Joints[JointType.ShoulderRight].Position.X;
            //tblPositionShoulderRightY.Text = "Position Right Shoulder Y: " + body.Joints[JointType.ShoulderRight].Position.Y;
            //tblPositionShoulderRightZ.Text = "Position Right Shoulder Z: " + body.Joints[JointType.ShoulderRight].Position.Z;
            //tblPositionElbowRightX.Text = "Position Rigth Elbow Y: " + body.Joints[JointType.ElbowRight].Position.X;
            //tblPositionElbowRightY.Text = "Position Rigth Elbow X: " + body.Joints[JointType.ElbowRight].Position.Y;
            //tblPositionElbowRightZ.Text = "Position Rigth Elbow Y: " + body.Joints[JointType.ElbowRight].Position.Z;
            
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

        private void Timer()
        {

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTicker;
            dt.Start();
        }

        private void dtTicker(object sender, EventArgs e)
        {
            string timerSec = incrementSeconds.ToString();
            string timerMin = incrementMinuts.ToString();
            ++incrementSeconds;
            
            if(incrementSeconds == 60)
            {
                incrementSeconds = 0;
                ++incrementMinuts;
            }
            if(incrementMinuts < 10)
            {
                timerMin = '0' + incrementMinuts.ToString();
            }
            if (incrementSeconds < 10)
            {
                timerSec = '0' + incrementSeconds.ToString();
            }
            
            //tblTimer.Content = timerMin.ToString() + ":" + timerSec.ToString();

        }
        
        private void UploadButton_Click(object sender, RoutedEvent e)
        {

        }

        private void PlayButton_Click(object sender, RoutedEvent e)
        {

        }

        private void ScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                anglesrules.CsvBuilder(body, viewer);
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



    }

}