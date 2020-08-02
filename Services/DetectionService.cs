using iris_server.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            IDictionary<string, object> analysis = new Dictionary<string, object>();
            analysis.Add("falldetection", DetectFalls(transformsJson));
            analysis.Add("movementdetection", DetectMovement(transformsJson));
            analysis.Add("confusiondetection", DetectConfusion(transformsJson));

            return analysis;
        }

        private static bool DetectFalls(JObject transformsJson)
        {
            List<float> yTransforms = transformsJson["yPos"].ToObject<List<float>>();
            const double THRESHOLD_VELOCITY = 0.3; // TODO: Get from config.
            IList<float> d1 = CalculateFirstDerivative(yTransforms);
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
            // TODO: INCOMPLETE IMPLEMENTATION
            // Looks for little-no head movement.
            const float ROT_THRESHOLD = 0.03f; // TODO: Get from config.
            const float POS_THRESHOLD = 0.03f; // TODO: Get from config.

            List<float> xTransforms = transformsJson["xPos"].ToObject<List<float>>();
            List<float> yTransforms = transformsJson["yPos"].ToObject<List<float>>();
            List<float> zTransforms = transformsJson["zPos"].ToObject<List<float>>();
            List<float> xRot = transformsJson["xRot"].ToObject<List<float>>();
            List<float> yRot = transformsJson["yRot"].ToObject<List<float>>();
            List<float> zRot = transformsJson["zRot"].ToObject<List<float>>();

            float pos = xTransforms.Sum() + yTransforms.Sum() + zTransforms.Sum();
            float rot = xRot.Sum() + yRot.Sum() + zRot.Sum();

            if (pos <= POS_THRESHOLD && rot < ROT_THRESHOLD)
            {
                return true; // Should detectconfusion();
            }
            return false;
        }

        public static bool DetectMovement(JObject transformsJson)
        {
            // Checks if the user has moved at least n metres.
            const float DISTANCE_THRESHOLD = 1.5f; // 1.5m 
            List<float> xTransforms = transformsJson["xPos"].ToObject<List<float>>();
            List<float> yTransforms = transformsJson["yPos"].ToObject<List<float>>();
            List<float> zTransforms = transformsJson["zPos"].ToObject<List<float>>();
            System.Numerics.Vector3 pos = new System.Numerics.Vector3(xTransforms.Sum(), yTransforms.Sum(), zTransforms.Sum());
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
