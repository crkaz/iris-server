using iris_server.Models;
using iris_server.Models.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace iris_server.Services
{
    public static class DetectionService
    {
        public static IDictionary<string, object> AnalyseMovement(JObject transformsJson)
        {
            IDictionary<string, object> analysis = new Dictionary<string, object>();

            List<float> xPosTransforms = transformsJson["xPos"].ToObject<List<float>>();
            List<float> yPosTransforms = transformsJson["yPos"].ToObject<List<float>>();
            List<float> zPosTransforms = transformsJson["zPos"].ToObject<List<float>>();
            List<float> xRotTransforms = transformsJson["xRot"].ToObject<List<float>>();
            List<float> yRotTransforms = transformsJson["yRot"].ToObject<List<float>>();
            List<float> zRotTransforms = transformsJson["zRot"].ToObject<List<float>>();

            // Detect falls with ypos
            analysis.Add("falldetection", DetectFalls(yPosTransforms));

            return analysis;
        }

        private static bool DetectFalls(IList<float> yTransforms)
        {
            const double THRESHOLD_VELOCITY = -0.5; // TODO: Get from config.
            IList<float> d1 = CalculateFirstDerivative(yTransforms);
            double maxChange = d1.Max();
            if (maxChange >= THRESHOLD_VELOCITY)
            {
                // TODO: then check if there was a recovery?
                return true;
            }
            return false;
        }

        public static bool DetectFall(JArray transformsJson)
        {
            //List<double> transforms = transformsJson.ToObject<List<double>>();
            //const double THRESHOLD_VELOCITY = 0.5; // TODO: Get from config.
            ////List<double> d2 = CalculateFirstDerivative(CalculateFirstDerivative(transforms));
            ////double maxChange = d2.Max();
            //List<double> d1 = CalculateFirstDerivative(transforms);
            //double maxChange = d1.Max();
            //if (maxChange >= THRESHOLD_VELOCITY)
            //{
            //    // TODO: then check if there was a recovery.
            //    return true;
            //}
            return false;
        }


        private static IList<float> CalculateFirstDerivative(IList<float> transforms)
        {
            IList<float> d1 = new List<float>();
            for (int i = 1; i < transforms.Count - 1; ++i)
            {
                d1.Add(transforms[i - 1] - transforms[i]);
            }
            return d1;
        }



        public static bool DetectConfusion(JObject transformsJson)
        {
            //const double THRESHOLD_DISTANCE = 0.2; // TODO: Get from config.
            //const double THRESHOLD_VERTICAL_ROTATION = 0.2; // TODO: Get from config.
            //const double THRESHOLD_HORIZONTAL_ROTATION = 0.2; // TODO: Get from config.

            //double rotationX;
            //double rotationY;
            //double distance;

            //// List of {posx,posy,posz,rotx,roty,rotz}
            //var transforms = JObject.FromObject(transformsJson).ToObject<Dictionary<string, object>>();

            //foreach (var transform in transforms)
            //{

            //}
            //double avgVerticalVelocity = avgVerticalVelocity = transforms.Average();

            //if (avgVerticalVelocity > THRESHOLD_VELOCITY)
            //{
            //    return true;
            //}

            return false;
        }


        public static bool DetectRoom(JObject transforms)
        {
            // TODO: Fall detection logic.
            return true;
        }
    }
}
