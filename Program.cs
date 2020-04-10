using System;

namespace voting_machine
{
/*
    Problem brief:
        On election day, a voting machine writes data in the form (voter_id, candidate_id) to a text file. 
        Write a program that reads this file as a stream and returns the top 3 candidates at any given time. 
        If you find a voter voting more than once, report this as fraud.
*/
    class Program
    {
        static void Main(string[] args)
        {
            var path = "./votes.txt";
            var voteWriter = new VoteWriter(path);
            var voteReader = new VoteReader(path);

            //Create driver data
            var rand = new Random();
            for (var i = 1; i < 50; i++)
            {
                //5 Potential candidates
                var randCandidateId = rand.Next(1, 6);
                voteWriter.writeVote(i, randCandidateId);
            }

            //Generate fraud data
            voteWriter.writeVote(0, 0);
            voteWriter.writeVote(0, 1);

            //Read votes into list
            var votes = voteReader.readVotes();
            //Count each candidates votes
            var candidateVotes = voteReader.countCandidateVotes(votes);

            //Print out top three candidates
            foreach (var candidate in voteReader.findTopThreeCandidates(candidateVotes))
            {
                Console.WriteLine("Candidate id: {0} - Votes: {1}", candidate.Key, candidate.Value);
            }
        }
    }
}
