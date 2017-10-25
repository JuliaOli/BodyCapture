using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CaptureBody
{
    class AngleRules
    {
        double height = 0;
        double left = 0;
        double right = 0;
        //trunk
        double trunk = 0;
        //hip flexion
        double rightHipFlexion = 0;
        double leftHipFlexion = 0;
        //neck 
        double neckFlexion = 0;
        double neckExtension = 0;
        //abducao de ombro
        double rightShoulderAbduction = 0;
        double leftShoulderAbduction = 0;
        //flexao de ombro
        double rightShoulderFlexion = 0;
        double leftShoulderFlexion = 0;
        //Checkbox
        bool elevationS;
        bool inadequateS;
        bool symetryN;
        bool rectifinedT;
        bool symetryT;
        //Timers
        string incrementStimer;
        string incrementNtimer;
        string incrementTtimer;

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
            //Hip flexion
            rightHipFlexion = Math.Round(body.RightHipFlexion(), 2);
            leftHipFlexion = Math.Round(body.LeftHipFlexion(), 2);
            //Trunk
            trunk = Math.Round(body.TrunkFlexion(), 2);
            //Flexao de ombro
            rightShoulderFlexion = Math.Round(body.RightShoulderFlexion(), 2);
            leftShoulderFlexion = Math.Round(body.LeftShoulderFlexion(), 2);
            //Abducao de ombro
            rightShoulderAbduction = Math.Round(body.RightShoulderAbduction(), 2);
            leftShoulderAbduction = Math.Round(body.LeftShoulderAbduction(), 2);
            //Neck
            neckFlexion = Math.Round(body.NeckFlexion(), 2);
            neckExtension = Math.Round(body.NeckExtension(), 2);

        }

        /// <Risks>
        /// Risks based on given angles that changes the variable color into the interface
        /// </Risks>
        /// <param name="variables"></param>
        /// <Colors></Colors>

        public Color shoulderAbductionRisk(double shoulderAbduction)
        {
            if(shoulderAbduction > 20)
            {
                return Colors.Purple;
            }
            else
            {
                return Colors.Green;
            }
        }

        public Color shoulderFlexionRisk(double shoulderFlextion)
        {

            if (shoulderFlextion > 35)
            {
                return Colors.Red;
            }
            else
            {
                return Colors.Green;
            }
        }

        public Color neckFlexionRisk(double neckFlextion)
        {

            if (neckFlexion > 25 || neckFlexion < 0)
            {
                return Colors.Red;
            }
            else
            {
                return Colors.Green;
            }
        }

        public Color trunkRisk(double trunk)
        {
            //Inclinação de tronco anterior <20° (verde), > 20° (vermelho)
            //Inclinação de tronco posterior < 20°(verde), > 20(vermelho)

            if (trunk > 20)
            {
                return Colors.Red;
            }
            else
            {
                return Colors.Green;
            }
        }

        public Color hipRisk(double hipAngle)
        {
            //Extensão / flexão de fêmur < 90 - 110° (verde) < 90° ou > 110° (vermelho)

            if (hipAngle > 110 || hipAngle < 90)
            {
                return Colors.Red;
            } 
            else
            {
                return Colors.Green;
            }
        }

        public bool colorCheck(Color color1)
        {
            if (color1 == Colors.Red)
            {
                return true;
            }
            return false;
        }

        //Get and Set

        public double Height
        {
            get => this.height;
        }
        public double Left
        {
            get => this.left;
        }
        public double Right
        {
            get => this.right;
        }
        public double Trunk
        {
            get => this.trunk;
        }
        public double RightHipFlexion
        {
            get => this.rightHipFlexion;
        }
        public double LeftHipFlexion
        {
            get => this.leftHipFlexion;
        }
        public double NeckFlexion
        {
            get => this.neckFlexion;
        }
        public double NeckExtension
        {
            get => this.neckExtension;
        }
        public double RightShoulderFlexion
        {
            get => this.rightShoulderFlexion;
        }
        public double LeftShoulderFlexion
        {
            get => this.leftShoulderFlexion;
        }
        public double RightShoulderAbduction
        {
            get => this.rightShoulderAbduction;
        }
        public double LeftShoulderAbduction
        {
            get => this.leftShoulderAbduction;
        }

        public string IncrementSTimer
        {
            get => this.incrementStimer;
            set => this.incrementStimer = value;
        }
        public string IncrementNTimer
        {
            get => this.incrementNtimer;
            set => this.incrementNtimer = value;
        }
        public string IncrementTTimer
        {
            get => this.incrementTtimer;
            set => this.incrementTtimer = value;
        }

        public bool ElevationS
        {
            get => this.elevationS;
            set => this.elevationS = value;
        }
        public bool InadequateS
        {
            get => this.inadequateS;
            set => this.inadequateS = value;
        }
        public bool SymetryN
        {
            get => this.symetryN;
            set => this.symetryN = value;
        }
        public bool RectifinedT
        {
            get => this.rectifinedT;
            set => this.rectifinedT = value;
        }
        public bool SymetryT
        {
            get => this.symetryT;
            set => this.symetryT = value;
        }

        /*
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
    if (elevationS)
    {
        aux = "Shoulder Elevation:; Not recommended";
    }
    else
    {
        aux = "Shoulder Elevation:; Acceptable";
    }
    csvContent.AppendLine(aux);

    if (inadequateS)
    {
        aux = "Inadequate Arm Posture:; Not recommended";
    }
    else
    {
        aux = "Inadequate Arm Posture:; Acceptable";
    }
    csvContent.AppendLine(aux);

    if (this.abductionRisk(rightShoulderAbduction) == Colors.Red)
    {
        aux = "Right Shoulder Abduction:;" + rightShoulderAbduction.ToString() + "°;" + "Time:" + incrementStimer ;
    }
    else
    {
        aux = "Right Shoulder Abduction:;" + rightShoulderAbduction.ToString() + "°;";
    }
    csvContent.AppendLine(aux);

    if (this.abductionRisk(leftShoulderAbduction) == Colors.Red)
    {
        aux = "Left Shoulder Abduction:;" + leftShoulderAbduction.ToString() + "°;" + "Time:" + incrementStimer;
    }
    else
    {
        aux = "Left Shoulder Abduction:;" + leftShoulderAbduction.ToString() + "°;";
    }
    csvContent.AppendLine(aux);

    aux = "Right Shoulder Flexion:;" + rightShoulderFlexion.ToString() + "°";
    csvContent.AppendLine(aux);
    aux = "Left Shoulder Flexion:;" + leftShoulderFlexion.ToString() + "°";
    csvContent.AppendLine(aux);


    //Neck
    if (symetryN)
    {
        aux = "Neck Symmetry: Not recommended";
    }
    else
    {
        aux = "Neck Symmetry: Acceptable";
    }

    aux = "Neck Flexion;" + neckFlexion.ToString() + " graus";
    csvContent.AppendLine(aux);
    aux = "Neck Extension;" + neckExtension.ToString() + " graus";
    csvContent.AppendLine(aux);

    //Trunk
    if (rectifinedT)
    {
        aux = "Rectified Spine: Not recommended";
    }
    else
    {
        aux = "Rectified Spine: Acceptable";
    }
    if (symetryT)
    {
        aux = "Trunk Symmetry: Not recommended";
    }
    else
    {
        aux = "Trunk Symmetry: Acceptable";
    }
    aux = "Trunk Flexion;" + hipFlexionRight.ToString() + " graus";
    csvContent.AppendLine(aux);
    aux = "Angle Hip Right;" + hipFlexionRight.ToString() + " graus";
    csvContent.AppendLine(aux);

    // Display height.
    aux = "Height;" + height.ToString() + "m";
    csvContent.AppendLine(aux);

    //Display Arms
    aux = "Left;" + left.ToString() + "m";
    csvContent.AppendLine(aux);
    aux = "Right;" + right.ToString() + "m";
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
        /*aux = "Position Left Shoulder X;" + body.Joints[JointType.ShoulderLeft].Position.X.ToString();
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
        csvContent.AppendLine(aux);*/

        //Right Shoulder and Elbow Positions
        /*
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

    return csvContent;

}

/// <CSVFile>
/// Builds the file with the csv and a printscreen from the body skeleton and rgb image
/// </summary>
/// <param name="body"></param>
/// <param name="viewer"></param>

public void CsvBuilder(Body body, KinectViewer viewer)
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
*/


        ///Change color based on risks
        ///
        /*
        public Color getRiskColor(double shoulderFlextion, double neckFlexion, double neckExtension) {

            //Risco 1
            //flexao do ombro > 35
            //flexao do pescoco > 25
            //extensao do pescoco < 0

            //Risco2
            //flexao do ombro > 35 e flexao do pescoco > 30
            //flexao do ombro > 35 e extensao do pescoco < 0

            if (shoulderFlextion > 35)
            {
                if (neckFlexion > 30 || neckExtension < 0)
                {
                    return Colors.Purple;
                }
                else
                {
                    return Colors.Red;
                }
            }
            else
            {
                return Colors.Green;
            }
        }
        */
    }
}
