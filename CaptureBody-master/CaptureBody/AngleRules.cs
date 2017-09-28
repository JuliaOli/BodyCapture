using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptureBody
{
    class AngleRules
    {
        double height = 0;
        double left = 0;
        double right = 0;
        double rightHipAngle = 0;
        double shoulderFlexion = 0;
        double neckFlexion = 0;
        double neckExtension = 0;


        /// <Body Angles Variables>
        /// Here the formulas to calculate the angles of the body parts are setted
        /// This variables will be used in the MainWindow class, where they will be plotted on the screen
        /// <Body Angles Variables>
        /// <param name="body"></param>
        /// 
        public AngleRules(Body body){

            height = Math.Round(body.UpperHeight(), 2);
            left = Math.Round(body.LeftHand(), 2);
            right = Math.Round(body.RightHand(), 2);
            rightHipAngle = Math.Round(body.HipRelativeAngle(), 2);
            shoulderFlexion = Math.Round(body.RightShoulderFlexion(), 2);
            neckFlexion = Math.Round(body.neckFlexion(), 2);
            neckExtension = Math.Round(body.neckExtension(), 2);

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
            aux = "Height;" + height.ToString() + "m";
            csvContent.AppendLine(aux);

            // Display height.

            //Display Arms
            aux = "Left;" + left.ToString() + "m";
            csvContent.AppendLine(aux);
            aux = "Right;" + right.ToString() + "m";
            csvContent.AppendLine(aux);
            
            aux = "Angle Hip Right;" + rightHipAngle.ToString() + " graus";
            csvContent.AppendLine(aux);
            aux = "Shoulder Flexion;" + shoulderFlexion.ToString() + " graus";
            csvContent.AppendLine(aux);
            aux = "Neck Flexion;" + neckFlexion.ToString() + " graus";
            csvContent.AppendLine(aux);
            aux = "Neck Extension;" + neckExtension.ToString() + " graus";
            csvContent.AppendLine(aux);
            
            try
            {
                // FileStream is IDisposable
                aux = "Position Head X;" + body.Joints[JointType.Head].Position.X.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Head Y;" + body.Joints[JointType.Head].Position.Y.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Head Z," + body.Joints[JointType.Head].Position.Z.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Right Shoulder Y;" + body.Joints[JointType.ShoulderRight].Position.Y.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Right Shoulder X;" + body.Joints[JointType.ShoulderRight].Position.X.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Right Shoulder Y;" + body.Joints[JointType.ShoulderRight].Position.Y.ToString();
                csvContent.AppendLine(aux).ToString();
                aux = "Position Rigth Elbow Y;" + body.Joints[JointType.ElbowRight].Position.Y.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Rigth Elbow X;" + body.Joints[JointType.ElbowRight].Position.X.ToString();
                csvContent.AppendLine(aux);
                aux = "Position Rigth Elbow Y;" + body.Joints[JointType.ElbowRight].Position.Y.ToString();
                csvContent.AppendLine(aux);
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Erro loading the file. " + "Variable body hasn't been set yet");
            }

            return csvContent;

        }
        



















    }
}
