using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GraniteCore;
using Newtonsoft.Json;

namespace GraniteCore.LocalFileRepository
{
    public class LocalFileRepository<TBaseEntityModel, TPrimaryKey> : IBaseRepository<TBaseEntityModel, TPrimaryKey>
        where TBaseEntityModel : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        private string _fileName = "data-sv-builder.txt";
        private readonly string _appName;

        public LocalFileRepository()
        {
            _appName = typeof(LocalFileRepository<,>).Assembly.FullName;
        }

        #region IRepository

        public virtual IQueryable<TBaseEntityModel> GetAll()
        {
            ICollection<TBaseEntityModel> collection = new List<TBaseEntityModel>();
            try
            {
                using (StreamReader sr = getStreamReaderForFileLocalFile())
                {
                    var line = sr.ReadLine();

                    while (line != null)
                    {
                        var model = JsonConvert.DeserializeObject<TBaseEntityModel>(line);

                        collection.Add(model);

                        line = sr.ReadLine();
                    }
                }

                return collection.AsQueryable();
            }
            catch (Exception)
            {
                return collection.AsQueryable();
            }
        }

        public virtual Task<TBaseEntityModel> Create(TBaseEntityModel entityModel)
        {
            return Task.Run(() =>
            {
                try
                {
                    var filePath = getFilePath();
                    using (StreamWriter outputFile = new StreamWriter(filePath, append: true))
                    {
                        var json = JsonConvert.SerializeObject(entityModel);
                        outputFile.WriteLine(json);
                    }
                    return entityModel;
                }
                catch (Exception)
                {
                    // todo
                    return entityModel;
                }
            });
        }

        public async virtual Task Delete(TPrimaryKey id)
        {
            try
            {
                string tempFile = Path.GetTempFileName();
                var entityToRemove = await GetByID(id);
                var filePath = getFilePath();
                using (var sr = new StreamReader(filePath))
                using (var sw = new StreamWriter(tempFile))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line != JsonConvert.SerializeObject(entityToRemove))
                            sw.WriteLine(line);
                    }
                }

                File.Delete(filePath);
                File.Move(tempFile, filePath);
            }
            catch (Exception)
            {
                // todo
            }
        }

        public async virtual Task Update(TPrimaryKey id, TBaseEntityModel entityModel)
        {
            try
            {
                var entity = await GetByID(id);

                string file = File.ReadAllText(getFilePath());

                file = file.Replace(JsonConvert.SerializeObject(entity), JsonConvert.SerializeObject(entityModel));

                File.WriteAllText(getFilePath(), file);

            }
            catch (Exception)
            {
                // todo
            }
        }

        public virtual Task<TBaseEntityModel> GetByID(TPrimaryKey id)
        {
            return Task.Run(() =>
            {
                TBaseEntityModel entity = default(TBaseEntityModel);
                try
                {
                    using (StreamReader sr = getStreamReaderForFileLocalFile())
                    {
                        var line = sr.ReadLine();

                        while (line != null && entity == null)
                        {
                            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<TBaseEntityModel>(line);
                            if (model != null && model.ID.Equals(id))
                                entity = model;

                            line = sr.ReadLine();
                        }
                    }
                    return entity;
                }
                catch (Exception)
                {
                    return entity;
                }
            });
        }

        public Task<TBaseEntityModel> GetByID(TPrimaryKey id, params Expression<Func<TBaseEntityModel, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        private StreamReader getStreamReaderForFileLocalFile()
        {
            return new StreamReader(getFilePath());
        }

        private string getFilePath()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");

            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            return Path.Combine(path, _fileName);
        }
        #endregion

    }
}
