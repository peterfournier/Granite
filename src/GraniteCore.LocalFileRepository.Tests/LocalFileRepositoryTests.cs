using GraniteCore.LocalFileRepository.IntegrationTests.Models;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GraniteCore.LocalFileRepository.IntegrationTests
{
    public class LocalFileRepositoryTests
    {
        private WorkoutEntityMock workoutEntity;
        private int workoutIdentity;

        [SetUp]
        public void Setup()
        {
            workoutIdentity = Guid.NewGuid().GetHashCode();
            if (workoutIdentity < 0)
                workoutIdentity = workoutIdentity * -1;

            workoutEntity = new WorkoutEntityMock()
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

            var success = await repository.Create(workoutEntity);

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

            await repository.Create(workoutEntity);

            var result = await repository.GetByID(workoutIdentity);
            Assert.AreEqual(workoutIdentity, result.ID);
        }

        [Test]
        public async Task Update_WithValidObject_UpdatesObject()
        {
            var repository = createDefaultLocalRepo();
            var toUpdate = await repository.Create(workoutEntity);
            string newWorkoutName = "Basic Class";
            toUpdate.Name = newWorkoutName;

            await repository.Update(toUpdate.ID, toUpdate);

            var result = await repository.GetByID(workoutEntity.ID);
            Assert.AreEqual(newWorkoutName, result.Name);
        }

        [Test]
        public async Task Remove_WithValidObject_UpdatesObject()
        {
            var repository = createDefaultLocalRepo();
            var workout = await repository.Create(workoutEntity);

            await repository.Delete(workout.ID);

            var result = await repository.GetByID(workoutEntity.ID);
            Assert.IsNull(result);
        }

        private static LocalFileRepository<WorkoutEntityMock, int> createDefaultLocalRepo()
        {
            return new LocalFileRepository<WorkoutEntityMock, int>();
        }
    }
}