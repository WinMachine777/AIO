using System.Collections.Generic;

namespace AIO.Common.Response
{

    public partial class State
    {
        public List<Values> colors { get; set; }
        public List<Values> numbers { get; set; }
        public List<Values> parities { get; set; }
        public List<Values> ranges { get; set; }
        public List<Values> rows { get; set; }

    }

    public partial class Values
    {
        public string value { get; set; }
        public decimal amount { get; set; }
    }

    public class rouletteBet
    {
        public string id { get; set; }
        public string iid { get; set; }
        public double payoutMultiplier { get; set; }
        public decimal amount { get; set; }
        public decimal payout { get; set; }
        public string updatedAt { get; set; }
        public string currency { get; set; }
        public string game { get; set; }
        public Active user { get; set; }
        public State state { get; set; }
    }
}
