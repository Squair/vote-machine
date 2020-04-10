using System.IO;
using System;

namespace voting_machine
{
    public class VoteWriter
    {
        //Path to votes.txt
        private string path { get; set; }

        //Constructor
        public VoteWriter(string path){
            this.path = path;
        }

        //Writes a vote into vote file path
        public void writeVote(int voter_id, int candidate_id)
        {
            //Try writeOperation, will evaluate to either File.CreateText or File.AppendText
            try
            {
                this.writeOperation(voter_id, candidate_id);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        //Gets the StreamWriter method based on wheter the file exists or not and returns the method
        private Func<string, StreamWriter> getWriteMethod()
        {
            if (File.Exists(this.path)){
                return File.AppendText;
            } else {
                return File.CreateText;
            } 
        }

        //Writes the vote to the file in the format "(voter_id, candidate_id)" as per the problem brief
        private void writeOperation(int voter_id, int candidate_id)
        {
            var writeMethod = getWriteMethod();
            using (StreamWriter file = writeMethod(this.path))
            {
                file.WriteLine("({0}, {1})", voter_id, candidate_id);
            }
        }
    }
}