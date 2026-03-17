using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Persistance;

namespace Courses.Tests.Common
{
    public class CoursesContextFactory
    {

        public static Guid RoleStudent = Guid.NewGuid();
        public static Guid RoleCouch = Guid.NewGuid();
        public static Guid RoleAdmin = Guid.NewGuid();

        public static Guid UserStudent = Guid.NewGuid();
        public static Guid UserCouch = Guid.NewGuid();
        public static Guid UserAdmin = Guid.NewGuid();

        public static Guid UserForDelete = Guid.NewGuid();
        public static Guid UserForUpdate = Guid.NewGuid();

        public static Guid CourseForDelete = Guid.NewGuid();
        public static Guid CourseForUpdate = Guid.NewGuid();

        public static Guid ReviewForDelete = Guid.NewGuid();
        public static Guid ReviewForUpdate = Guid.NewGuid();

        public static Guid ModuleForDelete = Guid.NewGuid();
        public static Guid ModuleForUpdate = Guid.NewGuid();

        public static Guid MaterialForDelete = Guid.NewGuid();
        public static Guid MaterialForUpdate = Guid.NewGuid();

        public static Guid TestForDelete = Guid.NewGuid();
        public static Guid TestForUpdate = Guid.NewGuid();

        public static Guid QuestionForDelete = Guid.NewGuid();
        public static Guid QuestionForUpdate = Guid.NewGuid();

        public static Guid AnswerForDelete = Guid.NewGuid();
        public static Guid AnswerForUpdate = Guid.NewGuid();

        public static Guid ProgressUserForDelete = Guid.NewGuid();
        public static Guid ProgressUserForUpdate = Guid.NewGuid();

        public static Guid ProgressModuleForDelete = Guid.NewGuid();
        public static Guid ProgressModuleForUpdate = Guid.NewGuid();

        public static Guid ProgressMaterialForDelete = Guid.NewGuid();
        public static Guid ProgressMaterialForUpdate = Guid.NewGuid();

        public static Guid TestResultForDelete = Guid.NewGuid();
        public static Guid TestResultForUpdate = Guid.NewGuid();

        public static Guid AnswersUserForDelete = Guid.NewGuid();
        public static Guid AnswersUserForUpdate = Guid.NewGuid();

        public static CoursesDbContext Create()
        {

            var options = new DbContextOptionsBuilder<CoursesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .ConfigureWarnings(warnings=> warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .Options;
            var context = new CoursesDbContext(options);
            context.Database.EnsureCreated();

            #region Roles
            context.Roles.AddRange(
                new Role
                {
                    Id = RoleAdmin,
                    CreatedAt =DateTime.Today,
                    RoleName = "Admin",
                },
                new Role
                {
                    Id = RoleCouch,
                    CreatedAt =DateTime.Today,
                    RoleName = "Couch",
                },
                new Role
                {
                    Id = RoleStudent,
                    CreatedAt =DateTime.Today,
                    RoleName = "Student",
                });
            #endregion

            #region Users
            context.Users.AddRange(
                new User
                {
                    Id = UserForDelete,
                    NameUser = "Delete",
                    Login = "Delete",
                    Email = "Delete@gmail.com",
                    HashPassword = "$2a$08$vNcO6RjYkF4wFsSFDdVtaeh/Ud0VGqEBsjPBA6LXaVNHRb9Utw1bG",
                    RoleId = RoleStudent,
                    CreatedAt =DateTime.Today,
                    PhoneNumber = "89304066793",
                    IsActive = true
                },
                new User
                {
                    Id = UserForUpdate,
                    NameUser = "Update",
                    Login = "Update",
                    Email = "Update@gmail.com",
                    HashPassword = "$2a$08$6ck8pNFNwW7EqmCJuJkfP.4DIrjiRnliQMzc2junMoUsz2sxeYaAG",
                    RoleId = RoleStudent,
                    CreatedAt =DateTime.Today,
                    PhoneNumber = "89304066793",
                    IsActive = true
                },
                new User
                {
                    Id = UserStudent,
                    NameUser = "Student",
                    Login = "Student",
                    Email = "Student@gmail.com",
                    HashPassword = "$2a$08$JG//sCIsJnxabiGTCPY2J.SdUiYvj80DJOvHM8z9o.Voc0W8ZR3OC",
                    RoleId = RoleStudent,
                    CreatedAt =DateTime.Today,
                    PhoneNumber = "89304066793",
                    IsActive = true
                },
                new User
                {
                    Id = UserCouch,
                    NameUser = "Couch",
                    Login = "Couch",
                    Email = "Couch@gmail.com",
                    HashPassword = "$2a$08$YVGkRoiMjJXAwo5N/vwb.OdHm8RPhr0btvZi9Jos9HOnfAhlA0/lu",
                    RoleId = RoleCouch,
                    CreatedAt =DateTime.Today,
                    PhoneNumber = "89304066793",
                    IsActive = true
                },
                new User
                {
                    Id = UserAdmin,
                    NameUser = "Admin",
                    Login = "Admin",
                    Email = "Admin@gmail.com",
                    HashPassword = "$2a$08$uDfgYxfViF6nJxTHJ3LnseIov2CriZU8OfvQt5Yav0QZbkhzwscne",
                    RoleId = RoleAdmin,
                    CreatedAt =DateTime.Today,
                    PhoneNumber = "89304066793",
                    IsActive = true
                });
            #endregion

            #region RefreshTokens
            context.RefreshTokens.AddRange(

                new RefreshToken
                {
                    Id = Guid.Parse("187FEA25-809C-407C-9E10-52036B1FD1D0"),
                    ResreshToken = Guid.Parse("7A8C5166-D90B-48BF-A0E9-DC5B3DA3FC2D"),
                    CreatedAt =DateTime.Today,
                    ExpiresIn =DateTime.Today.AddDays(30),
                    UserId = UserAdmin,
                },
                new RefreshToken
                {
                    Id = Guid.Parse("71088856-DBF8-467D-9C27-2CC3752FD5B8"),
                    ResreshToken = Guid.Parse("E53EF4AE-67EA-4E9B-AA54-20925271AAC1"),
                    CreatedAt =DateTime.Today,
                    ExpiresIn =DateTime.Today.AddDays(30),
                    UserId = UserCouch,
                },
                new RefreshToken
                {
                    Id = Guid.Parse("B13EEEFD-107F-4C77-AE43-60731E0E1AF4"),
                    ResreshToken = Guid.Parse("4A919D2C-830C-4A1C-A223-7EF77258FC59"),
                    CreatedAt =DateTime.Today,
                    ExpiresIn =DateTime.Today.AddDays(30),
                    UserId = UserStudent,
                });
            #endregion

            #region Courses 
            context.Courses.AddRange(
                 new Course
                 {
                     Id = CourseForDelete,
                     Title = "Delete course title",
                     Description = "Delete course description",
                     CreatedAt =DateTime.Today,
                     UpdateAt = null,
                     Rait = 2,
                     UserId = UserCouch,
                     Status = "Published"
                 },
                  new Course
                  {
                      Id = CourseForUpdate,
                      Title = "Update course title",
                      Description = "Update course description",
                      CreatedAt =DateTime.Today,
                      UpdateAt = null,
                      Rait = 2,
                      UserId = UserCouch,
                      Status = "Published"
                  },
                new Course
                {
                    Id = Guid.Parse("C6D49ACE-09A4-4ADB-BB4B-A1267F5173A8"),
                    Title = "First course title",
                    Description = "First course description",
                    CreatedAt =DateTime.Today,
                    UpdateAt = null,
                    Rait = 2,
                    UserId = UserCouch,
                    Status = "Draft"
                },
                new Course
                {
                    Id = Guid.Parse("C832A1DB-AC83-4250-B915-FF64AD8383B5"),
                    Title = "Second course title",
                    Description = "Second course description",
                    CreatedAt =DateTime.Today,
                    UpdateAt = null,
                    Rait = 4.5m,
                    UserId = UserAdmin,
                    Status = "Published"
                });
            #endregion

            #region Reviews
            context.Reviews.AddRange(
                new Review
                {
                    Id = ReviewForDelete,
                    UserId = UserStudent,
                    CourseId = Guid.Parse("C832A1DB-AC83-4250-B915-FF64AD8383B5"),
                    Rait = 5,
                    Text = "Удалить",
                    CreatedAt =DateTime.Today
                },
                new Review
                {
                    Id = ReviewForUpdate,
                    UserId = UserStudent,
                    CourseId = Guid.Parse("C832A1DB-AC83-4250-B915-FF64AD8383B5"),
                    Rait = 5,
                    Text = "Обновить",
                    CreatedAt =DateTime.Today
                },
                new Review
                {
                    Id = Guid.Parse("CE90037A-8F66-4CDC-97A7-C16B84F860D5"),
                    UserId = UserStudent,
                    CourseId = Guid.Parse("C832A1DB-AC83-4250-B915-FF64AD8383B5"),
                    Rait = 5,
                    Text = "Неплохой курс",
                    CreatedAt =DateTime.Today
                },
                 new Review
                 {
                     Id = Guid.Parse("713F37D4-CE49-450B-A1D0-F50612F5F320"),
                     UserId = UserCouch,
                     CourseId = Guid.Parse("C6D49ACE-09A4-4ADB-BB4B-A1267F5173A8"),
                     Rait = 2,
                     Text = "Плохой курс",
                     CreatedAt =DateTime.Today
                 },
                  new Review
                  {
                      Id = Guid.Parse("7D56030E-6BDD-4108-8FE3-F0DE138517FC"),
                      UserId = UserAdmin,
                      CourseId = Guid.Parse("C832A1DB-AC83-4250-B915-FF64AD8383B5"),
                      Rait = 4,
                      Text = "Хороший курс",
                      CreatedAt =DateTime.Today
                  });
            #endregion

            #region Modules
            context.Modules.AddRange(
                new Module
                {
                    Id = ModuleForDelete,
                    CourseId = Guid.Parse("C6D49ACE-09A4-4ADB-BB4B-A1267F5173A8"),
                    Title = "Delete title module of first course",
                    Description = "Delete description module of first course",
                    Order = 10,
                },
                new Module
                {
                    Id = ModuleForUpdate,
                    CourseId = Guid.Parse("C6D49ACE-09A4-4ADB-BB4B-A1267F5173A8"),
                    Title = "Update title module of first course",
                    Description = "Update description module of first course",
                    Order = 11,
                },
                new Module
                {
                    Id = Guid.Parse("FED57263-8338-42FA-82F7-D5752D914CE1"),
                    CourseId = Guid.Parse("C6D49ACE-09A4-4ADB-BB4B-A1267F5173A8"),
                    Title = "First title module of first course",
                    Description = "First description module of first course",
                    Order = 1,
                },
                new Module
                {
                    Id = Guid.Parse("B5293697-9BC5-4137-835A-39E3E0873FE5"),
                    CourseId = Guid.Parse("C832A1DB-AC83-4250-B915-FF64AD8383B5"),
                    Title = "First title module of second course",
                    Description = "First description module of second course",
                    Order = 1,
                });
            #endregion

            #region Materials
            context.Materials.AddRange(

               new Material
               {
                   Id = MaterialForDelete,
                   ModuleId = Guid.Parse("FED57263-8338-42FA-82F7-D5752D914CE1"),
                   Title = "Delete title material of first course",
                   Description = "Delete description material of first course",
                   Order = 10,
               },

               new Material
               {
                   Id = MaterialForUpdate,
                   ModuleId = Guid.Parse("FED57263-8338-42FA-82F7-D5752D914CE1"),
                   Title = "Update title material of first course",
                   Description = "Update description material of first course",
                   Order = 11,
               },
               new Material
               {
                   Id = Guid.Parse("C411E9B9-B6B5-4AC5-8CE3-34BE991BC910"),
                   ModuleId = Guid.Parse("FED57263-8338-42FA-82F7-D5752D914CE1"),
                   Title = "First title material of first course",
                   Description = "First description material of first course",
                   Order = 1,
               },
               new Material
               {
                   Id = Guid.Parse("A329C9A4-50B2-48E8-A213-8F6B64E15358"),
                   ModuleId = Guid.Parse("B5293697-9BC5-4137-835A-39E3E0873FE5"),
                   Title = "First title material of second course",
                   Description = "First description material of second course",
                   Order = 1,
               });
            #endregion

            #region Tests
            context.Tests.AddRange(
                new Test
                {
                    Id = TestForDelete,
                    MaterialId = Guid.Parse("C411E9B9-B6B5-4AC5-8CE3-34BE991BC910"),
                    Title = "Delete title test of first course",
                    Description = "Delete description test of first course",
                    Order = 10,
                },
                new Test
                {
                    Id = TestForUpdate,
                    MaterialId = Guid.Parse("C411E9B9-B6B5-4AC5-8CE3-34BE991BC910"),
                    Title = "Update title test of first course",
                    Description = "Update description test of first course",
                    Order = 11,
                },
               new Test
               {
                   Id = Guid.Parse("F019CC79-026F-4572-8E27-79A73495A8E7"),
                   MaterialId = Guid.Parse("C411E9B9-B6B5-4AC5-8CE3-34BE991BC910"),
                   Title = "First title test of first course",
                   Description = "First description test of first course",
                   Order = 1,
               },
               new Test
               {
                   Id = Guid.Parse("D5440AF1-AC4D-4DBA-A072-2A6B1A6F1FB7"),
                   MaterialId = Guid.Parse("A329C9A4-50B2-48E8-A213-8F6B64E15358"),
                   Title = "First title test of second course",
                   Description = "First description test of second course",
                   Order = 2,
               });
            #endregion

            #region Questions
            context.Questions.AddRange(
                new Question
                {
                    Id = QuestionForDelete,
                    TestId = Guid.Parse("F019CC79-026F-4572-8E27-79A73495A8E7"),
                    Text = "Delete question first course"
                },
                new Question
                {
                    Id = QuestionForUpdate,
                    TestId = Guid.Parse("F019CC79-026F-4572-8E27-79A73495A8E7"),
                    Text = "Update question first course"
                },
                new Question
                {
                    Id = Guid.Parse("2BD4F87B-F177-4C29-8A72-81AC7C3633DC"),
                    TestId = Guid.Parse("F019CC79-026F-4572-8E27-79A73495A8E7"),
                    Text = "Question first course"
                },
                new Question
                {
                    Id = Guid.Parse("893962C6-CB12-4D6C-A0B5-B3BB4669416F"),
                    TestId = Guid.Parse("D5440AF1-AC4D-4DBA-A072-2A6B1A6F1FB7"),
                    Text = "Question second course"
                });
            #endregion

            #region Answers
            context.Answers.AddRange(
                 new Answer
                 {
                     Id = AnswerForDelete,
                     QuestionId = Guid.Parse("2BD4F87B-F177-4C29-8A72-81AC7C3633DC"),
                     Text = "Delete answer for first course",
                     IsCorrect = true
                 },
                new Answer
                {
                    Id = AnswerForUpdate,
                    QuestionId = Guid.Parse("2BD4F87B-F177-4C29-8A72-81AC7C3633DC"),
                    Text = "Update answer for first course",
                    IsCorrect = true
                },
                new Answer
                {
                    Id = Guid.Parse("C0233D09-16DA-4D64-9282-617A1D4D14CF"),
                    QuestionId = Guid.Parse("2BD4F87B-F177-4C29-8A72-81AC7C3633DC"),
                    Text = "First answer for first course",
                    IsCorrect = true
                },
                new Answer
                {
                    Id = Guid.Parse("8F6DBD71-5590-4796-BB25-64070C1A6B62"),
                    QuestionId = Guid.Parse("2BD4F87B-F177-4C29-8A72-81AC7C3633DC"),
                    Text = "Second answer for first course",
                    IsCorrect = false
                },
                new Answer
                {
                    Id = Guid.Parse("FAF1B760-0F89-4F8F-A2B8-49B74CF1DB3B"),
                    QuestionId = Guid.Parse("893962C6-CB12-4D6C-A0B5-B3BB4669416F"),
                    Text = "Second answer for second course",
                    IsCorrect = true
                },
                new Answer
                {
                    Id = Guid.Parse("5326F851-4986-4668-95A9-C98EB000311C"),
                    QuestionId = Guid.Parse("893962C6-CB12-4D6C-A0B5-B3BB4669416F"),
                    Text = "Second answer for second course",
                    IsCorrect = false
                });
            #endregion

            #region ProgressUsers
            context.ProgressUsers.AddRange(
                new ProgressUser
                {
                    Id = ProgressUserForDelete,
                    CourseId = Guid.Parse("C6D49ACE-09A4-4ADB-BB4B-A1267F5173A8"),
                    UserId = UserCouch,
                    Status = "В прохождении",
                    StartedAt =DateTime.Today,
                    FineshedAt = null,
                },
                new ProgressUser
                {
                    Id = ProgressUserForUpdate,
                    CourseId = Guid.Parse("C6D49ACE-09A4-4ADB-BB4B-A1267F5173A8"),
                    UserId = UserCouch,
                    Status = "В прохождении",
                    StartedAt =DateTime.Today,
                    FineshedAt = null,
                },
                new ProgressUser
                {
                    Id = Guid.Parse("812268C6-5E26-4003-A11B-61C13D858735"),
                    CourseId = Guid.Parse("C6D49ACE-09A4-4ADB-BB4B-A1267F5173A8"),
                    UserId = UserStudent,
                    Status = "В прохождении",
                    StartedAt =DateTime.Today,
                    FineshedAt = null,
                });
            #endregion

            #region ProgressModules
            context.ProgressModules.AddRange(
                new ProgressModule
                {
                    Id = ProgressModuleForDelete,
                    ModuleId = Guid.Parse("FED57263-8338-42FA-82F7-D5752D914CE1"),
                    ProgressUserId = Guid.Parse("812268C6-5E26-4003-A11B-61C13D858735"),
                    UserId = UserCouch,
                    Status = "Не начат",
                    StartedAt = null,
                },
                new ProgressModule
                {
                    Id = ProgressModuleForUpdate,
                    ModuleId = Guid.Parse("FED57263-8338-42FA-82F7-D5752D914CE1"),
                    UserId = UserCouch,
                    ProgressUserId = Guid.Parse("812268C6-5E26-4003-A11B-61C13D858735"),
                    Status = "Не начат",
                    StartedAt = null,
                },
                new ProgressModule
                {
                    Id = Guid.Parse("0944F140-6A50-432D-9064-BCDF552D92D6"),
                    ModuleId = Guid.Parse("FED57263-8338-42FA-82F7-D5752D914CE1"),
                    ProgressUserId = Guid.Parse("812268C6-5E26-4003-A11B-61C13D858735"),
                    UserId = UserStudent,
                    Status = "Не начат",
                    StartedAt = null,
                });
            #endregion

            #region ProgressMaterials
            context.ProgressMaterials.AddRange(
                new ProgressMaterial
                {
                    Id = ProgressMaterialForDelete,
                    MaterialId = Guid.Parse("C411E9B9-B6B5-4AC5-8CE3-34BE991BC910"),
                    ProgressModuleId = Guid.Parse("0944F140-6A50-432D-9064-BCDF552D92D6"),
                    UserId = UserCouch,
                    Status = "Не начатa",
                    StartedAt = null,
                },
                new ProgressMaterial
                {
                    Id = ProgressMaterialForUpdate,
                    MaterialId = Guid.Parse("C411E9B9-B6B5-4AC5-8CE3-34BE991BC910"),
                    ProgressModuleId = Guid.Parse("0944F140-6A50-432D-9064-BCDF552D92D6"),
                    UserId = UserCouch,
                    Status = "Не начатa",
                    StartedAt = null,
                },
                new ProgressMaterial
                {
                    Id = Guid.Parse("4C27AAF3-CB96-46B2-86D6-65A23D733CB1"),
                    MaterialId = Guid.Parse("C411E9B9-B6B5-4AC5-8CE3-34BE991BC910"),
                    ProgressModuleId = Guid.Parse("0944F140-6A50-432D-9064-BCDF552D92D6"),
                    UserId = UserStudent,
                    Status = "Не начатa",
                    StartedAt = null,
                });
            #endregion

            #region TestResults
            context.TestResults.AddRange(
                new TestResult
                {
                    Id = TestResultForDelete,
                    ProgressMaterialId = Guid.Parse("4C27AAF3-CB96-46B2-86D6-65A23D733CB1"),
                    UserId = UserCouch,
                    TestId = Guid.Parse("F019CC79-026F-4572-8E27-79A73495A8E7"),
                    Score = 0,
                    IsPassed = false,
                    CompletedAt = null
                },
                new TestResult
                {
                    Id = TestResultForUpdate,
                    ProgressMaterialId = Guid.Parse("4C27AAF3-CB96-46B2-86D6-65A23D733CB1"),
                    UserId = UserCouch,
                    TestId = Guid.Parse("F019CC79-026F-4572-8E27-79A73495A8E7"),
                    Score = 0,
                    IsPassed = false,
                    CompletedAt = null
                },
                new TestResult
                {
                    Id = Guid.Parse("DF31BBD3-2818-49F6-A716-5AC8E033B55C"),
                    ProgressMaterialId = Guid.Parse("4C27AAF3-CB96-46B2-86D6-65A23D733CB1"),
                    UserId = UserStudent,
                    TestId = Guid.Parse("F019CC79-026F-4572-8E27-79A73495A8E7"),
                    Score = 0,
                    IsPassed = false,
                    CompletedAt = DateTime.Today
                });
            #endregion

            #region AnswersUsers
            context.AnswersUsers.AddRange(
                new AnswersUser
                {
                    Id = AnswersUserForDelete,
                    UserId = UserCouch,
                    TestResultId = Guid.Parse("DF31BBD3-2818-49F6-A716-5AC8E033B55C"),
                    QuestionId = Guid.Parse("2BD4F87B-F177-4C29-8A72-81AC7C3633DC"),
                    AnswerId = Guid.Parse("8F6DBD71-5590-4796-BB25-64070C1A6B62"),
                },
                new AnswersUser
                {
                    Id = AnswersUserForUpdate,
                    UserId = UserCouch,
                    TestResultId = Guid.Parse("DF31BBD3-2818-49F6-A716-5AC8E033B55C"),
                    QuestionId = Guid.Parse("2BD4F87B-F177-4C29-8A72-81AC7C3633DC"),
                    AnswerId = Guid.Parse("8F6DBD71-5590-4796-BB25-64070C1A6B62"),
                },
                new AnswersUser
                {
                    Id = Guid.Parse("519F30AD-F025-44B6-8CFF-066679D81004"),
                    UserId = UserStudent,
                    TestResultId = Guid.Parse("DF31BBD3-2818-49F6-A716-5AC8E033B55C"),
                    QuestionId = Guid.Parse("2BD4F87B-F177-4C29-8A72-81AC7C3633DC"),
                    AnswerId = Guid.Parse("8F6DBD71-5590-4796-BB25-64070C1A6B62"),
                });
            #endregion

            context.SaveChanges();
            return context;
        }

        public static void Destroy(CoursesDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
