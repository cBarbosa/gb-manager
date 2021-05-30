using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Reflection;
using System.Text;

namespace gb_manager.Data
{
    public class Persistence : PersistenceBase<Persistence>
    {
        public Persistence(
            IConfiguration _configuration
            , ILogger<Persistence> _logger)
            : base(_configuration, _logger)
        {

        }

        public static int SalvarNaBase(string tabela, object obj, PersistenceMetodos metodo)
        {
            try
            {
                using MySqlConnection db = new(ConnectionString);
                db.Open();

                //INSERT
                var propriedades = new StringBuilder();
                var valores = new StringBuilder();
                //UPDATE
                var valoresAlterados = new StringBuilder();
                //DELETE
                int valorid = 0;

                foreach (var p in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (metodo.Equals(PersistenceMetodos.Inserir))
                    {
                        if (p.GetValue(obj) != null && p.Name != "Notifications" && p.Name != "Invalid" && p.Name != "Valid" && !p.GetMethod.IsVirtual)
                        {
                            if (string.IsNullOrEmpty(propriedades.ToString()))
                            {
                                propriedades.Append(p.Name);
                                valores.Append("@" + p.Name);
                            }
                            else
                            {
                                propriedades.Append(", " + p.Name);
                                valores.Append(", @" + p.Name);
                            }
                        }
                    }
                    else if (metodo.Equals(PersistenceMetodos.Alterar))
                    {
                        if (p.GetValue(obj) != null && p.Name != "Notifications" && p.Name != "Invalid" && p.Name != "Valid" && !p.GetMethod.IsVirtual)
                        {
                            if (string.IsNullOrEmpty(valoresAlterados.ToString()))
                            {
                                valoresAlterados.Append(p.Name + "=" + "@" + p.Name);
                            }
                            else
                            {
                                valoresAlterados.Append(", " + p.Name + "=" + "@" + p.Name);
                            }
                        }
                    }
                    else if (metodo.Equals(PersistenceMetodos.Excluir))
                    {
                        if (p.GetValue(obj) != null && p.Name.Equals("id"))
                        {
                            valorid = int.Parse($"@{p.Name}");
                        }
                    }
                }

                int retorno = 0;
                if (metodo.Equals(PersistenceMetodos.Inserir))
                {
                    retorno = db.ExecuteScalar<int>($@"INSERT INTO {tabela} ({propriedades}) VALUES({valores});
                                    SELECT LAST_INSERT_ID();", obj);
                }
                else if (metodo.Equals(PersistenceMetodos.Alterar))
                {
                    retorno = db.ExecuteScalar<int>($@"UPDATE {tabela} SET {valoresAlterados} WHERE id = @Id;
                                    SELECT id From {tabela} WHERE id = @Id;", obj);
                }
                else if (metodo.Equals(PersistenceMetodos.Excluir))
                {
                    retorno = db.ExecuteScalar<int>($@"DELETE FROM {tabela} WHERE id = {valorid};
                                    SELECT 1;");
                }

                db.Close();

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool ExcluirLogicoDaBase(string tabela, int id)
        {
            try
            {
                using MySqlConnection db = new(ConnectionString);
                db.Open();

                bool retorno = db.ExecuteScalar<bool>($@"UPDATE {tabela} SET active = 0 WHERE id = {id};
                                        SELECT 1;");

                db.Close();

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public enum PersistenceMetodos
        {
            Inserir = 1,
            Alterar = 2,
            Excluir = 3,
            ExcluirLogicamente = 4
        }
    }
}