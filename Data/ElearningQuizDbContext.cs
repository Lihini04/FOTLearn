using ElearningQuiz.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElearningQuiz.Infrastructure.Data
{
    public class ElearningQuizDbContext : DbContext
    {
        public ElearningQuizDbContext(DbContextOptions<ElearningQuizDbContext> options) : base(options) { }

        public DbSet<Faculty> Faculties { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Batch> Batches { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<LecturerSubject> LecturerSubjects { get; set; } = null!;
        public DbSet<Quiz> Quizzes { get; set; } = null!;
        public DbSet<QuizBatchTarget> QuizBatchTargets { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<QuestionOption> QuestionOptions { get; set; } = null!;
        public DbSet<QuizAttempt> QuizAttempts { get; set; } = null!;
        public DbSet<StudentAnswer> StudentAnswers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // 1. INSTITUTIONAL HIERARCHY
            // ==========================================

            // Faculty Configuration
            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.HasKey(f => f.FacId);
                entity.Property(f => f.FacId).HasColumnName("Fac_id").HasMaxLength(7);
                entity.Property(f => f.FacName).HasColumnName("Fac_name").HasMaxLength(100).IsRequired();
                entity.Property(f => f.CreatedOn).HasColumnName("created_on").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            });

            // Department Configuration
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.DepId);
                entity.Property(d => d.DepId).HasColumnName("Dep_id").HasMaxLength(7);
                entity.Property(d => d.FacId).HasColumnName("Fac_id").HasMaxLength(7).IsRequired();
                entity.Property(d => d.DepName).HasColumnName("Dep_name").HasMaxLength(100).IsRequired();
                entity.Property(d => d.DepCode).HasColumnName("Dep_code").HasMaxLength(20).IsRequired();
                entity.Property(d => d.CreatedOn).HasColumnName("created_on").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(d => d.Faculty)
                      .WithMany(f => f.Departments)
                      .HasForeignKey(d => d.FacId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Departments_Faculties");
            });

            // Course Configuration
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.CourseId);
                entity.Property(c => c.CourseId).HasColumnName("Course_id").HasMaxLength(7);
                entity.Property(c => c.DepId).HasColumnName("Dep_id").HasMaxLength(7).IsRequired();
                entity.Property(c => c.CourseName).HasColumnName("Course_name").HasMaxLength(150).IsRequired();
                entity.Property(c => c.CourseCode).HasColumnName("Course_code").HasMaxLength(20).IsRequired();
                entity.Property(c => c.CreatedOn).HasColumnName("created_on").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(c => c.Department)
                      .WithMany(d => d.Courses)
                      .HasForeignKey(c => c.DepId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Courses_Departments");
            });

            // Batch Configuration
            modelBuilder.Entity<Batch>(entity =>
            {
                entity.HasKey(b => b.BatchId);
                entity.Property(b => b.BatchId).HasColumnName("Batch_id").HasMaxLength(50);
                entity.Property(b => b.CourseId).HasColumnName("Course_id").HasMaxLength(7).IsRequired();
                entity.Property(b => b.BatchCode).HasColumnName("Batch_code").HasMaxLength(50).IsRequired();
                entity.Property(b => b.BatchIntakeYear).HasColumnName("Batch_Intake_Year").IsRequired();
                entity.Property(b => b.CreatedOn).HasColumnName("created_on").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(b => b.Course)
                      .WithMany(c => c.Batches)
                      .HasForeignKey(b => b.CourseId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Batches_Courses");
            });

            // Subject Configuration
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(s => s.SubjectId);
                entity.Property(s => s.SubjectId).HasColumnName("Subject_id").HasMaxLength(50);
                entity.Property(s => s.CourseId).HasColumnName("Course_id").HasMaxLength(7).IsRequired();
                entity.Property(s => s.SubjectName).HasColumnName("Subject_name").HasMaxLength(150).IsRequired();
                entity.Property(s => s.SubjectCode).HasColumnName("Subject_code").HasMaxLength(20).IsRequired();
                entity.Property(s => s.CreatedOn).HasColumnName("created_on").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(s => s.Course)
                      .WithMany(c => c.Subjects)
                      .HasForeignKey(s => s.CourseId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Subjects_Courses");
            });

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.UserId).HasColumnName("User_id").HasMaxLength(50);
                entity.Property(u => u.DepartmentId).HasColumnName("Department_id").HasMaxLength(7);
                entity.Property(u => u.BatchId).HasColumnName("Batch_id").HasMaxLength(50);
                entity.Property(u => u.FirstName).HasColumnName("first_name").HasMaxLength(50).IsRequired();
                entity.Property(u => u.MiddleName).HasColumnName("middle_name").HasMaxLength(50);
                entity.Property(u => u.LastName).HasColumnName("last_name").HasMaxLength(50).IsRequired();
                entity.Property(u => u.Email).HasMaxLength(150).IsRequired();
                entity.Property(u => u.PasswordHash).HasMaxLength(255).IsRequired();
                entity.Property(u => u.MobileNo).HasColumnName("Mobile_no").HasMaxLength(20);
                entity.Property(u => u.CreatedOn).HasColumnName("created_on").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                // Enum conversion (UserRole -> string in DB)
                entity.Property(u => u.Role)
                      .HasConversion<string>()
                      .HasMaxLength(30)
                      .IsRequired();

                entity.HasOne(u => u.Department)
                      .WithMany(d => d.Users)
                      .HasForeignKey(u => u.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Users_Departments");

                entity.HasOne(u => u.Batch)
                      .WithMany(b => b.Students)
                      .HasForeignKey(u => u.BatchId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Users_Batches");
            });

            // LecturerSubject Junction Table Configuration
            modelBuilder.Entity<LecturerSubject>(entity =>
            {
                entity.HasKey(ls => ls.LecturerSubjectId);
                entity.Property(ls => ls.LecturerSubjectId).HasColumnName("LecturerSubject_id");
                entity.Property(ls => ls.UserId).HasColumnName("User_id").HasMaxLength(50).IsRequired();
                entity.Property(ls => ls.SubjectId).HasColumnName("Subject_id").HasMaxLength(50).IsRequired();
                entity.Property(ls => ls.CreatedOn).HasColumnName("created_on").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(ls => ls.Lecturer)
                      .WithMany(u => u.LecturerSubjects)
                      .HasForeignKey(ls => ls.UserId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_LecturerSubjects_Users");

                entity.HasOne(ls => ls.Subject)
                      .WithMany(s => s.LecturerSubjects)
                      .HasForeignKey(ls => ls.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_LecturerSubjects_Subjects");
            });

            // ==========================================
            // 2. QUIZ ENGINE
            // ==========================================

            // Quiz Configuration
            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.HasKey(q => q.QuizId);
                entity.Property(q => q.QuizId).HasColumnName("Quiz_id").HasMaxLength(15);
                entity.Property(q => q.SubjectId).HasColumnName("Subject_id").HasMaxLength(50).IsRequired();
                entity.Property(q => q.CreatedBy).HasMaxLength(7).IsRequired();
                entity.Property(q => q.CreatedForBatchId).HasMaxLength(50);
                entity.Property(q => q.Title).HasMaxLength(150).IsRequired();
                entity.Property(q => q.Description);
                entity.Property(q => q.DurationMinutes).HasDefaultValue(30);
                entity.Property(q => q.MaxAttempts).HasDefaultValue(1);
                entity.Property(q => q.TotalPoints).HasPrecision(5, 2).HasDefaultValue(0.00m);
                entity.Property(q => q.StartTime).IsRequired();
                entity.Property(q => q.EndTime).IsRequired();
                entity.Property(q => q.Status).HasMaxLength(20).HasDefaultValue("Draft").IsRequired();
                entity.Property(q => q.IsPublished).HasDefaultValue(false);
                entity.Property(q => q.CreatedOn).HasColumnName("created_on").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(q => q.Subject)
                      .WithMany()
                      .HasForeignKey(q => q.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Quizzes_Subjects");

                entity.HasOne(q => q.Creator)
                      .WithMany()
                      .HasForeignKey(q => q.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Quizzes_Users");

                entity.HasOne(q => q.TargetBatch)
                      .WithMany()
                      .HasForeignKey(q => q.CreatedForBatchId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Quizzes_Batches");
            });

            modelBuilder.Entity<QuizBatchTarget>(entity =>
            {
                entity.HasKey(target => new { target.QuizId, target.BatchId });
                entity.Property(target => target.QuizId).HasColumnName("Quiz_id").HasMaxLength(15);
                entity.Property(target => target.BatchId).HasColumnName("Batch_id").HasMaxLength(50);

                entity.HasOne(target => target.Quiz)
                      .WithMany(quiz => quiz.BatchTargets)
                      .HasForeignKey(target => target.QuizId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_QuizBatchTargets_Quizzes");

                entity.HasOne(target => target.Batch)
                      .WithMany()
                      .HasForeignKey(target => target.BatchId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_QuizBatchTargets_Batches");
            });

            // Question Configuration
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(q => q.QuestionId);
                entity.Property(q => q.QuestionId).HasColumnName("Question_id").HasMaxLength(15);
                entity.Property(q => q.QuizId).HasColumnName("Quiz_id").HasMaxLength(15).IsRequired();
                entity.Property(q => q.QuestionText).IsRequired();
                entity.Property(q => q.Points).HasPrecision(5, 2).HasDefaultValue(1.00m);
                entity.Property(q => q.CreatedOn).HasColumnName("created_on").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                // Enum conversion (QuestionType -> string in DB)
                entity.Property(q => q.QuestionType)
                      .HasConversion<string>()
                      .HasMaxLength(30)
                      .IsRequired();

                entity.HasOne(q => q.Quiz)
                      .WithMany(qz => qz.Questions)
                      .HasForeignKey(q => q.QuizId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Questions_Quizzes");
            });

            // QuestionOption Configuration
            modelBuilder.Entity<QuestionOption>(entity =>
            {
                entity.HasKey(qo => qo.OptionId);
                entity.Property(qo => qo.OptionId).HasColumnName("Option_id").HasMaxLength(15);
                entity.Property(qo => qo.QuestionId).HasColumnName("Question_id").HasMaxLength(7).IsRequired();
                entity.Property(qo => qo.OptionLetter).HasColumnType("CHAR(1)").IsRequired();
                entity.Property(qo => qo.OptionText).HasMaxLength(255).IsRequired();
                entity.Property(qo => qo.IsCorrect).HasDefaultValue(false);
                entity.Property(qo => qo.CreatedOn).HasColumnName("created_on").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(qo => qo.Question)
                      .WithMany(q => q.QuestionOptions)
                      .HasForeignKey(qo => qo.QuestionId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_QuestionOptions_Questions");
            });

            // QuizAttempt Configuration
            modelBuilder.Entity<QuizAttempt>(entity =>
            {
                entity.HasKey(qa => qa.AttemptId);
                entity.Property(qa => qa.AttemptId).HasColumnName("Attempt_id").HasMaxLength(15);
                entity.Property(qa => qa.QuizId).HasColumnName("Quiz_id").HasMaxLength(15).IsRequired();
                entity.Property(qa => qa.StudentId).HasMaxLength(15).IsRequired();
                entity.Property(qa => qa.StartTime).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
                entity.Property(qa => qa.SubmittedAt);
                entity.Property(qa => qa.Score).HasPrecision(5, 2).HasDefaultValue(0.00m);
                entity.Property(qa => qa.Status).HasMaxLength(20).HasDefaultValue("In Progress").IsRequired();
                entity.Property(qa => qa.IsGraded).HasDefaultValue(false);
                entity.Property(qa => qa.GradedAt);
                entity.Property(qa => qa.GradedBy).HasMaxLength(15);
                entity.Property(qa => qa.CreatedOn).HasColumnName("created_on").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(qa => qa.Quiz)
                      .WithMany(q => q.QuizAttempts)
                      .HasForeignKey(qa => qa.QuizId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_QuizAttempts_Quizzes");

                entity.HasOne(qa => qa.Student)
                      .WithMany()
                      .HasForeignKey(qa => qa.StudentId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_QuizAttempts_Students");

                entity.HasOne(qa => qa.Grader)
                      .WithMany()
                      .HasForeignKey(qa => qa.GradedBy)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_QuizAttempts_Graders");
            });

            // StudentAnswer Configuration
            modelBuilder.Entity<StudentAnswer>(entity =>
            {
                entity.HasKey(sa => sa.AnswerId);
                entity.Property(sa => sa.AnswerId).HasColumnName("Answer_id").HasMaxLength(20);
                entity.Property(sa => sa.AttemptId).HasColumnName("Attempt_id").HasMaxLength(15).IsRequired();
                entity.Property(sa => sa.QuestionId).HasColumnName("Question_id").HasMaxLength(15).IsRequired();
                entity.Property(sa => sa.SelectedOptionId).HasColumnName("SelectedOption_id").HasMaxLength(15);
                entity.Property(sa => sa.TextAnswer);
                entity.Property(sa => sa.IsCorrect).HasDefaultValue(false);
                entity.Property(sa => sa.PointsAwarded).HasPrecision(5, 2).HasDefaultValue(0.00m);
                entity.Property(sa => sa.Feedback);
                entity.Property(sa => sa.AnsweredAt).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
                entity.Property(sa => sa.CreatedOn).HasColumnName("created_on").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(sa => sa.Attempt)
                      .WithMany(a => a.StudentAnswers)
                      .HasForeignKey(sa => sa.AttemptId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_StudentAnswers_QuizAttempts");

                entity.HasOne(sa => sa.Question)
                      .WithMany(q => q.StudentAnswers)
                      .HasForeignKey(sa => sa.QuestionId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_StudentAnswers_Questions");

                entity.HasOne(sa => sa.SelectedOption)
                      .WithMany()
                      .HasForeignKey(sa => sa.SelectedOptionId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_StudentAnswers_QuestionOptions");
            });
        }
    }
}