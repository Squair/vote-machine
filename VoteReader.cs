using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System;

namespace voting_machine
{
    public class VoteReader
    {
        //Path to votes.txt
        private string path { get; set; }

        //Constructor
        public VoteReader(string path)
        {
            this.path = path;
        }

        //Read votes from path using a stream
        public List<Vote> readVotes()
        {
            var votes = new List<Vote>();
            //Open file as a stream
            using (StreamReader file = File.OpenText(this.path))
            {
                //Try match each indiviudal voting item
                string pattern = @"(\(\d+,\s\d+\))";
                var reg = new Regex(pattern, RegexOptions.Compiled);
                //Loop while there are still lines in the file
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    //For each match, map it to vote DTO and add into vote list
                    foreach (var match in reg.Matches(line))
                    {
                        votes.Add(mapVote(match.ToString()));
                    }
                }
            }
            return votes;
        }

        //Counts how much votes each individual candidate has
        public Dictionary<int, int> countCandidateVotes(List<Vote> votes)
        {
            //Dictionary maps candidate id to candidates number of votes
            Dictionary<int, int> candidateVotes = new Dictionary<int, int>();
            //Keep track of whos already voted
            List<int> knownVoterIds = new List<int>();

            foreach (var vote in votes)
            {
                //If voter id already appeared, skip the vote count
                if (knownVoterIds.Contains(vote.voter_id))
                {
                    Console.WriteLine("Voter with id {0} is fraudulant", vote.voter_id);
                    continue;
                }
                else //Add the voter to known voter ids
                {
                    knownVoterIds.Add(vote.voter_id);
                }

                //Try get value for candidate and increment them, otherwise add candidate into dictionary
                int voteCount;
                if (candidateVotes.TryGetValue(vote.candidate_id, out voteCount))
                {
                    candidateVotes[vote.candidate_id] = voteCount + 1;
                }
                else
                {
                    candidateVotes.Add(vote.candidate_id, 1);
                }
            }
            return candidateVotes;
        }

        //Returns the top three candidates with the most votes. If there are less than three candidates, order them and just return
        public IEnumerable<KeyValuePair<int, int>> findTopThreeCandidates(Dictionary<int, int> candidateVotes)
        {
            if (candidateVotes.Count < 3){
                return candidateVotes.OrderByDescending(x => x.Value);
            }
            return candidateVotes.OrderByDescending(x => x.Value).Take(3);
        }

        //Map the vote in votes.txt to a Vote object
        private Vote mapVote(string vote)
        {
            //Sanitise the string
            vote = vote.Replace("(", "");
            vote = vote.Replace(")", "");
            vote = vote.Replace(" ", "");
            
            //Split voterId and Candidate id apart
            var voteComponents = vote.Split(",");

            //Map vote components onto Vote object
            return new Vote()
            {
                voter_id = Int32.Parse(voteComponents[0]),
                candidate_id = Int32.Parse(voteComponents[1]),
            };
        }
    }
}