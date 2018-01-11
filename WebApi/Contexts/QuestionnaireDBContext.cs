using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Contexts
{
    public class QuestionnaireDBContext : DbContext
    {

        public QuestionnaireDBContext(DbContextOptions<QuestionnaireDBContext> options) : base(options) { }
        public DbSet<Questionnaire> Questionnaire { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<Questions> Questions { get; set; }

    }
}
