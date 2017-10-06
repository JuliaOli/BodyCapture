using System;
using Microsoft.Kinect;
using XnaGeometry;

namespace CaptureBody
{
    /// <summary>
    /// Provides some common functionality on skeletal data.
    /// </summary>
    public static class SkeletalExtensions
    {
        #region Public methods

        const double CONSTANT_RADIANUS_TO_GRAUS = 57.3;

        /// <summary>
        /// Returns the height of the specified skeleton.
        /// </summary>
        /// <param name="skeleton">The specified user skeleton.</param>
        /// <returns>The height of the skeleton in meters.</returns>
        public static double Height(Body body) // Skeleton type para Body
        {
            const double HEAD_DIVERGENCE = 0.1;

            //SUBSTITUIÇÕES
            /*JointType dos tipos ShoulderCenter = SpineShoulder, Spine = [SpineBase, SpineMid, SpineShoulder], HipCenter = SpineBase*/


            Joint head = body.Joints[JointType.Head];
            Joint neck = body.Joints[JointType.SpineShoulder];
            Joint spine = body.Joints[JointType.SpineMid];
            Joint waist = body.Joints[JointType.SpineBase];
            Joint hipLeft = body.Joints[JointType.HipLeft];
            Joint hipRight = body.Joints[JointType.HipRight];
            Joint kneeLeft = body.Joints[JointType.KneeLeft];
            Joint kneeRight = body.Joints[JointType.KneeRight];
            Joint ankleLeft = body.Joints[JointType.AnkleLeft];
            Joint ankleRight = body.Joints[JointType.AnkleRight];
            Joint footLeft = body.Joints[JointType.FootLeft];
            Joint footRight = body.Joints[JointType.FootRight];

            // Find which leg is tracked more accurately.
            int legLeftTrackedJoints = NumberOfTrackedJoints(hipLeft, kneeLeft, ankleLeft, footLeft);
            int legRightTrackedJoints = NumberOfTrackedJoints(hipRight, kneeRight, ankleRight, footRight);

            double legLength = legLeftTrackedJoints > legRightTrackedJoints ? Length(hipLeft, kneeLeft, ankleLeft, footLeft) : Length(hipRight, kneeRight, ankleRight, footRight);

            return Length(head, neck, spine, waist) + legLength + HEAD_DIVERGENCE;
        }

        /// <summary>
        /// Returns the left hand position of the specified skeleton.
        /// Acording Joint Hierarchy
        /// </summary>
        /// <param name="skeleton">The specified user skeleton.</param>
        /// <returns>The position of the left hand.</returns>
        public static double LeftHand(this Body body)
        {
            Joint shoulderLeft = body.Joints[JointType.ShoulderLeft];
            Joint elbowLeft = body.Joints[JointType.ElbowLeft];
            Joint wristLeft = body.Joints[JointType.WristLeft];
            Joint handLeft = body.Joints[JointType.HandLeft];

            //return Length(shoulderLeft, elbowLeft, wristLeft, handLeft);
            //returnL ength(hipLeft, handLeft);
            return Length(shoulderLeft, elbowLeft, wristLeft, handLeft);
        }

        /// <summary>
        /// O Ângulo relativo do braço é calculado usando a Lei dos Cossenos. Essa lei é simplesmmente um caso mais geral do Teorema 
        /// de Pitágoras e descreve a relação entre os lados de um triângulo. Os ângulos relativos podem ser calculados usando 
        /// e Lei dos Cossenos. Essa lei é simplesmente um caso mais geral do Teorema de Pitágoras e descreve a relação entre os 
        /// lados de um triângulo. Para nossos propósitos, o triângulo é constituído por dois segmentos (b e c) e uma linha (a)
        /// unindo a ponta distai de um segmento com a ponta proximal do outro.
        /// Então iremos calcular a distância Euclidiana do Ombro Esquerdo até o Cotovelo como Sendo (B). A distância do cotovelo 
        /// até o punho como sendo C. E a distância euclidiana do punho até o Ombro como sendo A.
        /// </summary>
        /// <param name="body">The specified user body.</param>
        /// <returns>The position of the left hand.</returns>
        public static double LeftArmRelativeAngle(this Body body)
        {
            //
            Joint shoulderLeft = body.Joints[JointType.ShoulderLeft];
            Joint elbowLeft = body.Joints[JointType.ElbowLeft];
            Joint wristLeft = body.Joints[JointType.WristLeft];

            double B = EuclidianDistance(shoulderLeft, elbowLeft);
            double C = EuclidianDistance(elbowLeft, wristLeft);
            double A = EuclidianDistance(shoulderLeft, wristLeft);

            //A formula da lei dos cosenos é:
            //a^2 = b^2 + c^2 - 2*b*c*cos(Teta)
            //que resulta em:
            //cos(teta) = (b^2 + c^2 - a^2)/2*b*c;
            double cosTeta = (Math.Pow(B, 2) + Math.Pow(C, 2) - Math.Pow(A, 2)) / 2 * B * C;

            //para encontrar o ângulo em radianos calculamos a inversa do coseno que teremos
            double radianos = Math.Acos(cosTeta);

            //para converter o ângulo em graus deve-se multiplicar pela constante 57,3;
            double graus = radianos * CONSTANT_RADIANUS_TO_GRAUS;


            //return Length(shoulderLeft, elbowLeft, wristLeft, handLeft);
            //returnL ength(hipLeft, handLeft);
            return graus;
            //return 6;
        }

        /// <summary>
        /// 
        /// </summary>
        /* MATLAB - CODE
         * shoulder = [torsoJoint(Index,2),torsoJoint(Index,3), torsoJoint(Index,4)];
           wrist = [shoulderJoint(Index,2),shoulderJoint(Index,3), shoulderJoint(Index,4)];
           hip = [elbowJoint(Index,2),elbowJoint(Index,3), elbowJoint(Index,4)];
           u = wrist-shoulder;
           v = hip-shoulder;

           CosTheta = dot(u,v)/(norm(u)*norm(v));
           ThetaInDegrees = acos(CosTheta)*180/pi;
           angle = ThetaInDegrees;
        */
        public static double RightArmRelativeAngle(this Body body)
        {
            Joint shoulder = body.Joints[JointType.ShoulderRight];
            Joint wrist = body.Joints[JointType.WristRight];
            Joint hip = body.Joints[JointType.HipRight];
            //Vector3 v = new Vector3(3,3,3);
            Vector3 u = new Vector3(wrist.Position.X - shoulder.Position.X, wrist.Position.Y - shoulder.Position.Y,
                wrist.Position.Z - shoulder.Position.Z);
            Vector3 v = new Vector3(hip.Position.X - shoulder.Position.X, hip.Position.Y - shoulder.Position.Y,
                hip.Position.Z - shoulder.Position.Z);

            Double cosTheta = Vector3.Dot(Vector3.Normalize(u), Vector3.Normalize(v));

            Double ThetaInDegrees = Math.Acos(cosTheta) * 180 / Math.PI;


            return ThetaInDegrees;
        }

        public static double neckFlexion(this Body body)
        {
            Joint head = body.Joints[JointType.Head];
            Joint shoulderCenter = body.Joints[JointType.SpineShoulder]; //Shoulder = SpineShoulder
            Joint spine = body.Joints[JointType.SpineMid]; // Spine = SpineMid

            return scalarProduct(head, shoulderCenter, spine);
        }

        public static double neckExtension(this Body body)
        {
            return neckFlexion(body) - 180;
        }

        public static double scalarProduct(Joint x, Joint y, Joint z)
        {
            Vector3 u = new Vector3(x.Position.X - y.Position.X, x.Position.Y - y.Position.Y,
                x.Position.Z - y.Position.Z);
            Vector3 v = new Vector3(z.Position.X - y.Position.X, z.Position.Y - y.Position.Y,
                z.Position.Z - y.Position.Z);

            Double cosTheta = Vector3.Dot(Vector3.Normalize(u), Vector3.Normalize(v));

            Double ThetaInDegrees = Math.Acos(cosTheta) * 180 / Math.PI;

            return ThetaInDegrees;
        }


        public static double TrunkFlexion(this Body body)
        {
            Joint spineBase = body.Joints[JointType.SpineBase];
            Joint spineShoulder = body.Joints[JointType.SpineShoulder];
            Joint spineShoulder2 = new Joint();

            spineShoulder2.Position.X = spineBase.Position.X;
            spineShoulder2.Position.Z = spineBase.Position.Z;
            spineShoulder2.Position.Y = spineShoulder.Position.Y;
            

            //Vector3 v = new Vector3(3,3,3);
            Vector3 u = new Vector3(spineBase.Position.X - spineShoulder2.Position.X, spineBase.Position.Y - spineShoulder2.Position.Y,
                0);
            Vector3 v = new Vector3(spineBase.Position.X - spineShoulder.Position.X, spineBase.Position.Y - spineShoulder.Position.Y, 
                0);

            Double cosTheta = Vector3.Dot(Vector3.Normalize(u), Vector3.Normalize(v));

            Double ThetaInDegrees = Math.Acos(cosTheta) * 180 / Math.PI;

            return ThetaInDegrees;

        }

        //eixos y e z
        public static double RightShoulderFlexion(this Body body)
        {
            Joint rightElbow = body.Joints[JointType.ElbowRight];
            Joint rightShoulder = body.Joints[JointType.ShoulderRight];
            Joint rightHip = body.Joints[JointType.HipRight];


            //Vector3 v = new Vector3(3,3,3);
            Vector3 u = new Vector3(0, rightElbow.Position.Y - rightShoulder.Position.Y,
                rightElbow.Position.Z - rightShoulder.Position.Z);
            Vector3 v = new Vector3(0, rightHip.Position.Y - rightShoulder.Position.Y,
                rightHip.Position.Z - rightShoulder.Position.Z);

            Double cosTheta = Vector3.Dot(Vector3.Normalize(u), Vector3.Normalize(v));

            Double ThetaInDegrees = Math.Acos(cosTheta) * 180 / Math.PI;

            return ThetaInDegrees;
        }

        //eixos y e z
        public static double LeftShoulderFlexion(this Body body)
        {
            Joint leftElbow = body.Joints[JointType.ElbowLeft];
            Joint leftShoulder = body.Joints[JointType.ShoulderLeft];
            Joint leftHip = body.Joints[JointType.HipLeft];


            //Vector3 v = new Vector3(3,3,3);
            Vector3 u = new Vector3(0, leftElbow.Position.Y - leftShoulder.Position.Y,
                leftElbow.Position.Z - leftShoulder.Position.Z);
            Vector3 v = new Vector3(0, leftHip.Position.Y - leftShoulder.Position.Y,
                leftHip.Position.Z - leftShoulder.Position.Z);

            Double cosTheta = Vector3.Dot(Vector3.Normalize(u), Vector3.Normalize(v));

            Double ThetaInDegrees = Math.Acos(cosTheta) * 180 / Math.PI;

            return ThetaInDegrees;
        }

        //Adicionando abducao
        //eixos x e y
        public static double RightShoulderAbduction(this Body body)
        {
            Joint rightElbow = body.Joints[JointType.ElbowRight];
            Joint rightShoulder = body.Joints[JointType.ShoulderRight];
            Joint rightHip = body.Joints[JointType.HipRight];


            //Vector3 v = new Vector3(3,3,3);
            Vector3 u = new Vector3(rightElbow.Position.X - rightShoulder.Position.X, rightElbow.Position.Y - rightShoulder.Position.Y,
                0);
            Vector3 v = new Vector3(rightHip.Position.X - rightShoulder.Position.X, rightHip.Position.Y - rightShoulder.Position.Y,
                0);

            Double cosTheta = Vector3.Dot(Vector3.Normalize(u), Vector3.Normalize(v));

            Double ThetaInDegrees = Math.Acos(cosTheta) * 180 / Math.PI;

            return ThetaInDegrees;

            
        }


        //Adicionando abducao
        //eixos x e y
        public static double LeftShoulderAbduction(this Body body)
        {
            Joint leftElbow = body.Joints[JointType.ElbowLeft];
            Joint leftShoulder = body.Joints[JointType.ShoulderLeft];
            Joint leftHip = body.Joints[JointType.HipLeft];


            //Vector3 v = new Vector3(3,3,3);
            Vector3 u = new Vector3(leftElbow.Position.X - leftShoulder.Position.X, leftElbow.Position.Y - leftShoulder.Position.Y,
                0);
            Vector3 v = new Vector3(leftHip.Position.X - leftShoulder.Position.X, leftHip.Position.Y - leftShoulder.Position.Y,
                0);

            Double cosTheta = Vector3.Dot(Vector3.Normalize(u), Vector3.Normalize(v));

            Double ThetaInDegrees = Math.Acos(cosTheta) * 180 / Math.PI;

            return ThetaInDegrees;
        }

        //Baseado no artigo:http://www.efdeportes.com/efd182/condicoes-de-trabalho-de-um-setor-de-secretaria.htm
        //Figura: Avaliação postural por meio do Software de Avaliação Postura
        public static double HipRelativeAngle(this Body body)
        {
            Joint shoulder = body.Joints[JointType.ShoulderRight];
            Joint hipRight = body.Joints[JointType.HipRight];
            Joint kneeRight = body.Joints[JointType.KneeRight];


            //Vector3 v = new Vector3(3,3,3);
            Vector3 u = new Vector3(shoulder.Position.X - hipRight.Position.X, shoulder.Position.Y - hipRight.Position.Y,
                shoulder.Position.Z - hipRight.Position.Z);
            Vector3 v = new Vector3(kneeRight.Position.X - hipRight.Position.X, kneeRight.Position.Y - hipRight.Position.Y,
                kneeRight.Position.Z - hipRight.Position.Z);

            Double cosTheta = Vector3.Dot(Vector3.Normalize(u), Vector3.Normalize(v));

            

            Double ThetaInDegrees = Math.Acos(cosTheta) * 180 / Math.PI;

            return ThetaInDegrees;
        }


        public static double RightHand(this Body body)
        {
            Joint shoulderRight = body.Joints[JointType.ShoulderRight];
            //Joint hipCenter = body.Joints[JointType.HipCenter];
            //Joint elbowLeft = body.Joints[JointType.ElbowLeft];
            //Joint wristLeft = body.Joints[JointType.WristLeft];
            Joint handRight = body.Joints[JointType.HandRight];

            //return Length(shoulderLeft, elbowLeft, wristLeft, handLeft);
            //returnL ength(hipLeft, handLeft);
            return Length(shoulderRight, handRight);
        }

        /// <summary>
        /// Returns the upper height of the specified body (head to waist). Useful whenever Kinect provides a way to track seated users.
        /// </summary>
        /// <param name="body">The specified user body.</param>
        /// <returns>The upper height of the body in meters.</returns>
        public static double UpperHeight(this Body body)
        {
            Joint head = body.Joints[JointType.Head];
            Joint neck = body.Joints[JointType.SpineShoulder];                          //Shoulder = SpineShoulder
            Joint spine = body.Joints[JointType.SpineMid];                             //Spine = SpineMid
            Joint waist = body.Joints[JointType.SpineBase];                           //HipCenter = SpineBase

            return Length(head, neck, spine, waist);
        }

        /// <summary>
        /// Returns the length of the segment defined by the specified joints.
        /// </summary>
        /// <param name="p1">The first joint (start of the segment).</param>
        /// <param name="p2">The second joint (end of the segment).</param>
        /// <returns>The length of the segment in meters.</returns>
        public static double Length(Joint p1, Joint p2)
        {
            return Math.Sqrt(
                Math.Pow(p1.Position.X - p2.Position.X, 2) +
                Math.Pow(p1.Position.Y - p2.Position.Y, 2) +
                Math.Pow(p1.Position.Z - p2.Position.Z, 2));
        }

        /// <summary>
        /// Returns the Euclidian Distance of the segment defined by the specified joints.
        /// </summary>
        /// <param name="p1">The first joint (start of the segment).</param>
        /// <param name="p2">The second joint (end of the segment).</param>
        /// <returns>The length of the segment in meters.</returns>
        public static double EuclidianDistance(Joint p1, Joint p2)
        {
            return Math.Sqrt(
                Math.Pow(p1.Position.X - p2.Position.X, 2) +
                Math.Pow(p1.Position.Y - p2.Position.Y, 2));
        }

        /// <summary>
        /// Returns the length of the segments defined by the specified joints.
        /// </summary>
        /// <param name="joints">A collection of two or more joints.</param>
        /// <returns>The length of all the segments in meters.</returns>
        public static double Length(params Joint[] joints)
        {
            double length = 0;

            for (int index = 0; index < joints.Length - 1; index++)
            {
                length += Length(joints[index], joints[index + 1]);
            }

            return length;
        }

        /// <summary>
        /// Given a collection of joints, calculates the number of the joints that are tracked accurately.
        /// </summary>
        /// <param name="joints">A collection of joints.</param>
        /// <returns>The number of the accurately tracked joints.</returns>
        public static int NumberOfTrackedJoints(params Joint[] joints)
        {
            int trackedJoints = 0;

            foreach (var joint in joints)
            {
                if (joint.TrackingState == TrackingState.Tracked)  //JointTrackingState.Tracked  -> TrackingState.Tracked
                {
                    trackedJoints++;
                }
            }

            return trackedJoints;
        }

        /// <summary>
        /// Scales the specified joint according to the specified dimensions.
        /// </summary>
        /// <param name="joint">The joint to scale.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="skeletonMaxX">Maximum X.</param>
        /// <param name="skeletonMaxY">Maximum Y.</param>
        /// <returns>The scaled version of the joint.</returns>
        public static Joint ScaleTo(this Joint joint, int width, int height, float skeletonMaxX, float skeletonMaxY)
        {
            CameraSpacePoint point = new CameraSpacePoint();
            point.X = Scale(width, skeletonMaxX, joint.Position.X);
            point.Y = Scale(height, skeletonMaxY, -joint.Position.Y);
            point.Z = joint.Position.Z;
            /*
            SkeletonPoint position = new SkeletonPoint()
            {
                X = Scale(width, skeletonMaxX, joint.Position.X),
                Y = Scale(height, skeletonMaxY, -joint.Position.Y),
                Z = joint.Position.Z
            };
            */

            joint.Position = point;

            return joint;
        }

        /// <summary>
        /// Scales the specified joint according to the specified dimensions.
        /// </summary>
        /// <param name="joint">The joint to scale.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <returns>The scaled version of the joint.</returns>
        public static Joint ScaleTo(this Joint joint, int width, int height)
        {
            return ScaleTo(joint, width, height, 1.0f, 1.0f);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns the scaled value of the specified position.
        /// </summary>
        /// <param name="maxPixel">Width or height.</param>
        /// <param name="maxSkeleton">Border (X or Y).</param>
        /// <param name="position">Original position (X or Y).</param>
        /// <returns>The scaled value of the specified position.</returns>
        private static float Scale(int maxPixel, float maxSkeleton, float position)
        {
            float value = ((((maxPixel / maxSkeleton) / 2) * position) + (maxPixel / 2));

            if (value > maxPixel)
            {
                return maxPixel;
            }

            if (value < 0)
            {
                return 0;
            }

            return value;
        }

        #endregion
    }
}
