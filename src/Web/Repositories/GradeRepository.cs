using STKBC.Stats.Data.Models;

namespace STKBC.Stats.Repositories;

public interface IGradeRepository
{
    List<Grade> GetGrades();
    bool AddGrade(Grade grade);
}

public class InMemoryGradeRepository : IGradeRepository
{
    private List<Grade> _grades;

    public InMemoryGradeRepository(List<Grade>? grades = null)
    {
        _grades = grades ?? new List<Grade>();
    }

    public List<Grade> GetGrades()
    {
        return _grades;
    }

    public bool AddGrade(Grade grade)
    {
        _grades.Add(grade);
        return true;
    }


    public static InMemoryGradeRepository New()
    {
        var grades = new List<Grade>{
            new Grade {
                Id = new Guid("ebc452d2-6d8a-4374-971c-b3edb4dfcc43"),
                Name = "B",
                Key = "b1",
                LeagueId = InMemoryLeagueRepository.MWBL.Id,
            },
            new Grade {
                Id = new Guid("d2b6177d-f9ca-4293-8803-cc4ad9e79235"),
                Name = "B Reserve",
                Key = "b2",
                LeagueId = InMemoryLeagueRepository.MWBL.Id,
            },
             new Grade {
                Id = new Guid("c8c7bb35-c1a4-4a3d-a116-301846fd5086"),
                Name = "C",
                Key = "c1",
                LeagueId = InMemoryLeagueRepository.MWBL.Id,
            },
            new Grade {
                Id = new Guid("6957a7b2-9cd2-43eb-8096-e01996d6b9b9"),
                Name = "C Reserve",
                Key = "c2",
                LeagueId = InMemoryLeagueRepository.MWBL.Id,
            },
        };


        return new InMemoryGradeRepository();
    }
}