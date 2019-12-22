using System.Collections.Generic;

namespace RankedTyping.Responses
{
    public class TestResponse
    {
        public int id { get; set; }
        public int language_id { get; set; }
        public int test_type_id { get; set; }
        public string[] words { get; set; }
    }
}