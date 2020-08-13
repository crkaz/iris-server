using iris_server.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace iris_server.Services
{
    /// <summary>
    /// Methods dedicated to detection and analysis logic (e.g. fall, room, image) for use
    /// by the compute controller.
    /// </summary>
    public static class DetectionService
    {
        public static IDictionary<string, object> AnalyseMovement(JObject transformsJson)
        {
            const string FALL_DETECTION_KEY = "falldetection";
            const string ROOM_DETECTION_KEY = "movementdetection";
            const string CONFUSION_DETECTION_KEY = "confusiondetection";

            IDictionary<string, object> analysis = new Dictionary<string, object>();
            analysis.Add(FALL_DETECTION_KEY, DetectFalls(transformsJson));
            analysis.Add(ROOM_DETECTION_KEY, DetectMovement(transformsJson));
            analysis.Add(CONFUSION_DETECTION_KEY, DetectConfusion(transformsJson));

            return analysis;
        }


        private static bool DetectFalls(JObject transformsJson)
        {
            List<float> yTransforms = transformsJson["yPos"].ToObject<List<float>>();
            const double THRESHOLD_VELOCITY = 0.3; // TODO: Get from config.

            // Get the first derivate of the transforms.
            IList<float> d1 = CalculateFirstDerivative(yTransforms);

            // Compare the max value from the derivative to the threshold.
            double maxChange = d1.Max();
            if (maxChange >= THRESHOLD_VELOCITY)
            {
                // TODO: then check if there was a recovery?
                return true;
            }
            return false;
        }


        public static bool DetectConfusion(JObject transformsJson)
        {
            /// TODO: INCOMPLETE IMPLEMENTATION

            // Looks for little-no head movement.
            const float WINDOW_SIZE = 10.0f; // TODO: Confirm.
            const float ROT_THRESHOLD = 0.03f; // TODO: Get from config.
            const float POS_THRESHOLD = 0.3f; // TODO: Get from config.

            // Get transforms from json.
            List<float> xPos = transformsJson["xPos"].ToObject<List<float>>();
            List<float> yPos = transformsJson["yPos"].ToObject<List<float>>();
            List<float> zPos = transformsJson["zPos"].ToObject<List<float>>();
            List<float> xRot = transformsJson["xRot"].ToObject<List<float>>();
            List<float> yRot = transformsJson["yRot"].ToObject<List<float>>();
            List<float> zRot = transformsJson["zRot"].ToObject<List<float>>();

            // Sum transforms in vector form.
            Vector3 pos = new Vector3(xPos.Sum(), yPos.Sum(), zPos.Sum());
            Vector3 rot = new Vector3(xRot.Sum(), yRot.Sum(), zRot.Sum());

            // TODO:
            // 1. Split transforms into groups (e.g. 2 windows of 5 sec)
            // 2. Check if magnitudes change between the windows
            // 3. If not, detect confusion.

            // Compare vector magnitudes with thresholds.
            if (pos.Length() <= POS_THRESHOLD && rot.Length() < ROT_THRESHOLD)
            {
                return true; // Should detectconfusion();
            }
            return false;
        }


        public static bool DetectMovement(JObject transformsJson)
        {
            // Checks if the user has moved at least n metres.
            const float DISTANCE_THRESHOLD = 1.5f; // 1.5m 

            // Get transforms from json.
            List<float> xTransforms = transformsJson["xPos"].ToObject<List<float>>();
            List<float> yTransforms = transformsJson["yPos"].ToObject<List<float>>();
            List<float> zTransforms = transformsJson["zPos"].ToObject<List<float>>();

            // Sum transforms in vector form.
            Vector3 pos = new Vector3(xTransforms.Sum(), yTransforms.Sum(), zTransforms.Sum());

            // Compare vector magnitude with thresholds.
            if (pos.Length() >= DISTANCE_THRESHOLD)
            {
                return true; // Should detectroom().
            }
            return false;


        }


        public static async Task<string> DetectRoom(byte[] imageBytes)
        {
            // TODO: implement machine learning for decision making (i.e. room classification).
            try
            {
                // Rooms of interest. TODO: // context sensitive prompts from config
                List<string> roomTags = new List<string>() { "kitchen", "bedroom", "bathroom", "living room", "hallway" };

                var response = await AzureVisionService.Analyse(imageBytes);
                VisionTags tags = JsonConvert.DeserializeObject<VisionTags>(response);

                VisionTags.TagData detectedRoom = new VisionTags.TagData() { name = "unknown", confidence = 0.0f };
                foreach (var tag in tags.description.tags)
                {
                    if (roomTags.Contains(tag.ToLower()))
                    {
                        detectedRoom.name = tag;
                    }
                }

                return detectedRoom.name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "unknown";
            }
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
    }
}
