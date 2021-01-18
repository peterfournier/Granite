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
    public class LocalFileRepository<TDtoModel, TEntity, TPrimaryKey> : IBaseRepository<TDtoModel, TEntity, TPrimaryKey>
        where TDtoModel : IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        private string _fileName = "data-sv-builder.txt";
        private readonly string _appName;

        public LocalFileRepository()
        {
            _appName = typeof(LocalFileRepository<,,>).Assembly.FullName;
        }

        #region IRepository

        public virtual IQueryable<TDtoModel> GetAll()
        {
            ICollection<TDtoModel> collection = new List<TDtoModel>();
            try
            {
                using (StreamReader sr = getStreamReaderForFileLocalFile())
                {
                    var line = sr.ReadLine();

                    while (line != null)
                    {
                        var model = JsonConvert.DeserializeObject<TDtoModel>(line);

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

        public virtual Task<TDtoModel> Create(TDtoModel dtoModel)
        {
            return Task.Run(() =>
            {
                try
                {
                    var filePath = getFilePath();
                    using (StreamWriter outputFile = new StreamWriter(filePath, append: true))
                    {
                        var json = JsonConvert.SerializeObject(dtoModel);
                        outputFile.WriteLine(json);
                    }
                    return dtoModel;
                }
                catch (Exception)
                {
                    // todo
                    return dtoModel;
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

        public async virtual Task Update(TPrimaryKey id, TDtoModel dtoModel)
        {
            try
            {
                var entity = await GetByID(id);

                string file = File.ReadAllText(getFilePath());

                file = file.Replace(JsonConvert.SerializeObject(entity), JsonConvert.SerializeObject(dtoModel));

                File.WriteAllText(getFilePath(), file);

            }
            catch (Exception)
            {
                // todo
            }
        }

        public virtual Task<TDtoModel> GetByID(TPrimaryKey id)
        {
            return Task.Run(() =>
            {
                TDtoModel entity = default(TDtoModel);
                try
                {
                    using (StreamReader sr = getStreamReaderForFileLocalFile())
                    {
                        var line = sr.ReadLine();

                        while (line != null && entity == null)
                        {
                            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<TDtoModel>(line);
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

        public Task<TDtoModel> GetByID(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includeProperties)
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
