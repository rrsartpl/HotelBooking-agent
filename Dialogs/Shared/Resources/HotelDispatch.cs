// <auto-generated>
// Code generated by LUISGen HotelDispatch.json -cs Luis.HotelDispatch -o Dialogs\Shared\Resources
// Tool github: https://github.com/microsoft/botbuilder-tools
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Bot.Builder;
namespace Luis
{
    public class HotelDispatch: IRecognizerConvert
    {
        public string Text;
        public string AlteredText;


        // TODO: each hotelbot will have unique unique fb id --> 
        // enum for each? 2^32 is max
        // max dispatch sources: 500
        public enum Intent {
            l_HotelBot, 
            q_2129612763787673_en, 
            q_2129612763787673_nl,
            None

        };
        public Dictionary<Intent, IntentScore> Intents;

        public class _Entities
        {

            // Instance
            public class _Instance
            {
            }
            [JsonProperty("$instance")]
            public _Instance _instance;
        }
        public _Entities Entities;

        [JsonExtensionData(ReadData = true, WriteData = true)]
        public IDictionary<string, object> Properties {get; set; }

        public void Convert(dynamic result)
        {
            var app = JsonConvert.DeserializeObject<HotelDispatch>(JsonConvert.SerializeObject(result));
            Text = app.Text;
            AlteredText = app.AlteredText;
            Intents = app.Intents;
            Entities = app.Entities;
            Properties = app.Properties;
        }

        public (Intent intent, double score) TopIntent()
        {
            Intent maxIntent = Intent.None;
            var max = 0.0;
            foreach (var entry in Intents)
            {
                if (entry.Value.Score > max)
                {
                    maxIntent = entry.Key;
                    max = entry.Value.Score.Value;
                }
            }
            return (maxIntent, max);
        }
    }
}
