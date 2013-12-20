using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doctor_cms.Classes.Objects
{
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime PublishedDate { get; set; }
        public int Status { get; set; }
    }
}