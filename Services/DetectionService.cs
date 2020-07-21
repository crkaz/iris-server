using iris_server.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace iris_server.Services
{
    public static class DetectionService
    {
        public static bool DetectFall(JObject transformsJson, PatientConfig config)
        {
            const double THRESHOLD_VELOCITY = 0.5; // TODO: Get from config.
            var transforms = JObject.FromObject(transformsJson).ToObject<List<double>>(); // List of vertical positions (posY).
            //List<double> d2 = CalculateFirstDerivative(CalculateFirstDerivative(transforms));
            //double maxChange = d2.Max();
            List<double> d1 = CalculateFirstDerivative(transforms);
            double maxChange = d1.Max();
            if (maxChange >= THRESHOLD_VELOCITY)
            {
                return true;
            }
            return false;
        }


        private static List<double> CalculateFirstDerivative(List<double> transforms)
        {
            List<double> d1 = new List<double>();
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
