using Candidate_BusinessObject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Candidate_DAOs
{
    public class CandidateProfileDAO
    {
        private List<JobPosting> jobPostings;
        private ArrayList candidateProfiles;
        private static CandidateProfileDAO instance;
        private string filePath = "candidateProfiles.txt";
        private string postingFilePath = "jobPostings.txt";

        public static CandidateProfileDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CandidateProfileDAO();
                }
                return instance;
            }
        }

        public CandidateProfileDAO()
        {
            candidateProfiles = new ArrayList();
            jobPostings = LoadJobPostings();
            LoadDataFromFile();
        }

        private void LoadDataFromFile()
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines.Skip(1))
                {
                    var data = line.Split('\t');
                    if (data.Length >= 6)
                    {
                        var candidateProfile = new CandidateProfile
                        {
                            CandidateId = data[0],
                            Fullname = data[1],
                            Birthday = DateTime.Parse(data[2]),
                            ProfileShortDescription = data[3],
                            ProfileUrl = data[4],
                            PostingId = data[5]
                        };
                        candidateProfiles.Add(candidateProfile);
                    }
                }
            }
        }

        private List<JobPosting> LoadJobPostings()
        {
            var postings = new List<JobPosting>();
            if (File.Exists(postingFilePath))
            {
                var lines = File.ReadAllLines(postingFilePath);
                foreach (var line in lines.Skip(1))
                {
                    var data = line.Split('\t');
                    if (data.Length >= 2)
                    {
                        var jobPosting = new JobPosting
                        {
                            PostingId = data[0],
                            JobPostingTitle = data[1]
                        };
                        postings.Add(jobPosting);
                    }
                }
            }
            return postings;
        }

        public CandidateProfile GetCandidateProfile(string id)
        {
            return candidateProfiles.Cast<CandidateProfile>().SingleOrDefault(c => c.CandidateId.Equals(id));
        }

        public bool AddCandidateProfile(CandidateProfile candidateProfile)
        {
            bool isSuccess = false;
            CandidateProfile candidate = GetCandidateProfile(candidateProfile.CandidateId);
            if (candidate == null)
            {
                candidateProfiles.Add(candidateProfile);
                SaveDataToFile();
                isSuccess = true;
            }
            return isSuccess;
        }

        public bool DeleteCandidateProfile(string candidateID)
        {
            bool isSuccess = false;
            CandidateProfile candidate = GetCandidateProfile(candidateID);
            if (candidate != null)
            {
                candidateProfiles.Remove(candidate);
                SaveDataToFile();
                isSuccess = true;
            }
            return isSuccess;
        }

        public bool UpdateCandidateProfile(CandidateProfile candidateProfile)
        {
            bool isSuccess = false;
            CandidateProfile existingCandidate = GetCandidateProfile(candidateProfile.CandidateId);
            if (existingCandidate != null)
            {
                candidateProfiles.Remove(existingCandidate);
                candidateProfiles.Add(candidateProfile);
                SaveDataToFile();
                isSuccess = true;
            }
            return isSuccess;
        }

        private void SaveDataToFile()
        {
            var lines = candidateProfiles.Cast<CandidateProfile>()
                                         .Select(cp => $"{cp.CandidateId}\t{cp.Fullname}\t{cp.Birthday:yyyy-MM-dd HH:mm:ss.fff}\t{cp.ProfileShortDescription}\t{cp.ProfileUrl}\t{cp.PostingId}");
            File.WriteAllLines(filePath, new[] { "CandidateId\tName\tDateOfBirth\tSkills\tFilePath\tPostingId" }.Concat(lines));
        }

        public List<CandidateProfile> GetCandidatesWithPostings()
        {
            foreach (CandidateProfile candidate in candidateProfiles)
            {
                var posting = jobPostings.SingleOrDefault(p => p.PostingId.ToString() == candidate.PostingId);
                candidate.Posting = posting;
            }

            return candidateProfiles.Cast<CandidateProfile>().ToList();
        }
    }
}
