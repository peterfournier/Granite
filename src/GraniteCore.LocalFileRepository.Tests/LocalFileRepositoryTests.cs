using GraniteCore.LocalFileRepository.IntegrationTests.Models;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GraniteCore.LocalFileRepository.IntegrationTests
{
    public class LocalFileRepositoryTests
    {
        private Workout workoutDTO;
        private int workoutIdentity;

        [SetUp]
        public void Setup()
        {
            workoutIdentity = Guid.NewGuid().GetHashCode();
            if (workoutIdentity < 0)
                workoutIdentity = workoutIdentity * -1;

            workoutDTO = new Workout()
            {
                Name = "Master Class",
                Description = "You are you own Gym workout",
                ID = workoutIdentity
            };
        }

        [Test]
        public async Task Create_WithValidObject_CreatesJsonFile()
        {
            var repository = createDefaultLocalRepo();

            var success = await repository.Create(workoutDTO);

            Assert.IsNotNull(success);
        }

        [Test]
        public void Get_ReturnsObject()
        {
            var repository = createDefaultLocalRepo();

            var result = repository.GetAll().FirstOrDefault();

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Get_WithValidID_ReturnsObject()
        {
            var repository = createDefaultLocalRepo();

            await repository.Create(workoutDTO);

            var result = await repository.GetByID(workoutIdentity);
            Assert.AreEqual(workoutIdentity, result.ID);
        }

        [Test]
        public async Task Update_WithValidObject_UpdatesObject()
        {
            var repository = createDefaultLocalRepo();
            var toUpdate = await repository.Create(workoutDTO);
            string newWorkoutName = "Basic Class";
            toUpdate.Name = newWorkoutName;

            await repository.Update(toUpdate.ID, toUpdate);

            var result = await repository.GetByID(workoutDTO.ID);
            Assert.AreEqual(newWorkoutName, result.Name);
        }

        [Test]
        public async Task Remove_WithValidObject_UpdatesObject()
        {
            var repository = createDefaultLocalRepo();
            var workout = await repository.Create(workoutDTO);

            await repository.Delete(workout.ID);

            var result = await repository.GetByID(workoutDTO.ID);
            Assert.IsNull(result);
        }

        private static LocalFileRepository<Workout, WorkoutEntityMock, int> createDefaultLocalRepo()
        {
            return new LocalFileRepository<Workout, WorkoutEntityMock, int>();
        }
    }
}