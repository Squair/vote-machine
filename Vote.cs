
namespace voting_machine
{
    //DTO class to map strings from vote path into objects
    public class Vote
    {
        public int voter_id { get; set; }
        public int candidate_id { get; set; }
    }
}