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
        private int incrementSSeconds = 0;
        private int incrementSMinuts = 0;
        private int incrementNSeconds = 0;
        private int incrementNMinuts = 0;
        private int incrementTSeconds = 0;
        private int incrementTMinuts = 0;

        //Timer Checker
        bool recorrenceTrunk = false; //Globalcheck
        bool recorrenceNeck = false; //Globalcheck
        bool recorrenceShoulder = false; //Globalcheck
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
        }

        /// <summary>
        /// Método redundante... ele é implementado em angleRules
        /// mas as variáveis de angleRules não estão sendo usadas na interface...
        /// </summary>
        /// <param name="body"></param>
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
        }

        void Sensor_SkeletonFrameReady()
        {

            AngleVariables(body);
            anglesrules = new AngleRules(body);
            try
            {
                Checkboxes();
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Variables wasn't atributed");
            }
            Color colorAux1;
            Color colorAux2;

            //As variáveis que estão sendo usadas no display não são as mesmas usadas no csv?
            //talvez tenha um atraso ou um adiantamento nas variaveis usadas pois elas são diferentes...
            //precisa analisar se isso é um problema ou só redundância...


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
            incrementHip = anglesrules.colorCheck(colorAux1, colorAux1);

            //Trunk

            colorAux1 = anglesrules.trunkRisk(trunk);
            colorAux2 = anglesrules.trunkRisk(trunk);
            tblTrunk.Text = "Anterior trunk inclination" + trunk.ToString() + "º";
            tblTrunk.Foreground = new SolidColorBrush(colorAux1);
            incrementTrunk = anglesrules.colorCheck(colorAux1, colorAux1);

            if(incrementHip || incrementTrunk)
            {
                if(recorrenceTrunk == false)
                {
                    TrunkTimer();
                    recorrenceTrunk = true;
                }
                
            }
            else
            {
                recorrenceTrunk = false;
                incrementTMinuts = 0;
                incrementTSeconds = 0;
            }
            
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

            if(incrementFlex || incrementAbd)
            {
                if (recorrenceShoulder == false)
                {
                    ShoulderTimer();
                    recorrenceShoulder = true;
                }
            }
            else
            {
                recorrenceShoulder = false;
                incrementSSeconds = 0;
                incrementSMinuts = 0;
            }

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

            if (incrementNeckFlex || incrementNeckExt)
            {
                if (recorrenceNeck == false)
                {
                    NeckTimer();
                    recorrenceNeck = true;
                }
            }
            else
            {
                recorrenceNeck = false;
                incrementNSeconds = 0;
                incrementNMinuts = 0;
            }
            
            //Display bodyPositions
            //Display Head positions
            tblPositionHeaderX.Text = "Position Head X: " + body.Joints[JointType.Head].Position.X;
            tblPositionHeaderY.Text = "Position Head Y: " + body.Joints[JointType.Head].Position.Y;
            tblPositionHeaderZ.Text = "Position Head Z: " + body.Joints[JointType.Head].Position.Z;
            
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


        /// <Timers>
        /// There must be an optimized way of doing it without replicate all the timer code
        /// for all the variables, but they must be setted separately
        /// </Timers Code>
        
        private void ShoulderTimer()
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtSTicker;
            dt.Start();
        }

        private void dtSTicker(object sender, EventArgs e)
        {
            string timerSec = incrementSSeconds.ToString();
            string timerMin = incrementSMinuts.ToString();
            ++incrementSSeconds;
            
            if(incrementSSeconds == 60)
            {
                incrementSSeconds = 0;
                ++incrementSMinuts;
            }
            if(incrementSMinuts < 10)
            {
                timerMin = '0' + incrementSMinuts.ToString();
            }
            if (incrementSSeconds < 10)
            {
                timerSec = '0' + incrementSSeconds.ToString();
            }

            tblShoulderTimer.Content = timerMin.ToString() + ":" + timerSec.ToString();
            anglesrules.IncrementSTimer = timerMin.ToString() + ":" + timerSec.ToString();
        }

        private void NeckTimer()
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTTicker;
            dt.Start();
        }

        private void dtNTicker(object sender, EventArgs e)
        {
            string timerSec = incrementNSeconds.ToString();
            string timerMin = incrementNMinuts.ToString();
            ++incrementNSeconds;

            if (incrementNSeconds == 60)
            {
                incrementNSeconds = 0;
                ++incrementNMinuts;
            }
            if (incrementNMinuts < 10)
            {
                timerMin = '0' + incrementNMinuts.ToString();
            }
            if (incrementNSeconds < 10)
            {
                timerSec = '0' + incrementNSeconds.ToString();
            }

            tblNeckTimer.Content = timerMin.ToString() + ":" + timerSec.ToString();
            anglesrules.IncrementNTimer = timerMin.ToString() + ":" + timerSec.ToString();

        }

        private void TrunkTimer()
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTTicker;
            dt.Start();
        }

        private void dtTTicker(object sender, EventArgs e)
        {
            ++incrementTSeconds;
            string timerSec = incrementTSeconds.ToString();
            string timerMin = incrementTMinuts.ToString();

            if (incrementTSeconds == 60)
            {
                incrementTSeconds = 0;
                ++incrementTMinuts;
            }
            if (incrementTMinuts < 10)
            {
                timerMin = '0' + incrementTMinuts.ToString();
            }
            if (incrementTSeconds < 10)
            {
                timerSec = '0' + incrementTSeconds.ToString();
            }

            tblTrunkTimer.Content = timerMin.ToString() + ":" + timerSec.ToString();
            //anglesrules.IncrementTTimer = timerMin.ToString() + ":" + timerSec.ToString();

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

        public void Checkboxes(){

            
            if ((bool)ElevationS.IsChecked == true) //왜 이레...
            {
                anglesrules.ElevationS = true;
            }
            else
            {
                anglesrules.ElevationS = false;
            }

            if ((bool)InadequateS.IsChecked == true) //왜 이레...
            {
                anglesrules.InadequateS = true;
            }
            else
            {
                anglesrules.InadequateS = false;
            }

            if ((bool)SymetryN.IsChecked == true) //왜 이레...
            {
                anglesrules.SymetryN = true;
            }
            else
            {
                anglesrules.SymetryN = false;
            }

            if ((bool)RectifinedT.IsChecked == true) //왜 이레...
            {
                anglesrules.RectifinedT = true;
            }
            else
            {
                anglesrules.RectifinedT = false;
            }

            if ((bool)SymetryT.IsChecked == true) //왜 이레...
            {
                anglesrules.SymetryT = true;
            }
            else
            {
                anglesrules.SymetryT = false;
            }
        }

    }

}