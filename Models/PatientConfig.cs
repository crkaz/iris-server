using iris_server.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iris_server.Models
{
    public class PatientConfig
    {
        public enum Features { voiceinput, gestureinput, gazeinput, falldetection, roomdetection, confusiondetection, stickynotes };

        // DB fields/
        [Key]
        public string Id { get; set; } // Primary key.
        [ForeignKey("PatientId")]
        public string PatientId { get; set; }
        public IList<IFeature> EnabledFeatures { get; set; }
        // TODO: add complex config options; potentially require individual config models.

        [Timestamp] // Enable optimistic concurrency measures by timestamping transactions (EF convention).
        public byte[] RowVersion { get; set; }

        public PatientConfig() { }
    }
}
