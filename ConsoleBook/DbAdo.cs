﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ConsoleLibrary.DataEntity;

namespace ConsoleLibrary
{
    public class DbAdo
    {
        // data
        private string _conn;

        // Constructor
        public DbAdo()
        {
            _conn = this.GetConnectionString();
        }

        public DbAdo(string conn)
        {
            _conn = conn;
        }

        // methods
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DBCONN"].ConnectionString;
        }

        #region Role CRUD
        public List<Role> GetRole()
        {
            List<Role> _list = new List<Role>();

            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spGetRole", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;
                    //_sqlCommand.Parameters.AddWithValue("@BookID", inOneParticularBook);

                    con.Open();
                    Role _role;
                    using (SqlDataReader reader = _sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _role = new Role
                            {
                                RoleID = reader.GetInt32(reader.GetOrdinal("RoleID")),
                                RoleName = (string)reader["RoleName"]
                                //Description = (string)reader["Book_Description"],
                                //Price = reader.GetDecimal(reader.GetOrdinal("Book_Price")),
                                //IsPaperback = (string)reader["Book_IsPaperBack"],
                                //Author_FK = reader.GetInt32(reader.GetOrdinal("Book_AuthorID_FK")),
                                //Genre_FK = reader.GetInt32(reader.GetOrdinal("GenreID_FK"))
                            };
                            _list.Add(_role);
                        }
                    }
                    con.Close();
                }
            }
            return _list;
        }

        public int CreateRole(Role r)
        {

            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spCreateRole", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;
                    //_sqlCommand.Parameters.AddWithValue("@ParamRoleName", r.RoleName);
                    //_sqlCommand.Parameters.Add("@ParamRoleName", SqlDbType.NVarChar(100)).Value = r.RoleName;
                    SqlParameter _paramRoleName = _sqlCommand.CreateParameter();
                    _paramRoleName.DbType = DbType.String;
                    _paramRoleName.ParameterName = "@ParamRoleName";
                    _paramRoleName.Value = r.RoleName;
                    _sqlCommand.Parameters.Add(_paramRoleName);

                    SqlParameter _paramRoleIDReturn = _sqlCommand.CreateParameter();
                    _paramRoleIDReturn.DbType = DbType.Int32;
                    _paramRoleIDReturn.ParameterName = "@ParamOutRoleID";
                    var pk = _sqlCommand.Parameters.Add(_paramRoleIDReturn);
                    _paramRoleIDReturn.Direction = ParameterDirection.Output;

                    con.Open();
                    _sqlCommand.ExecuteNonQuery();   // calls the sp 
                    var result = _paramRoleIDReturn.Value;
                    con.Close();
                    return (int)result;
                }
            }
        }

        public void DeleteRole(Role r)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spDeleteRole", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;
                    SqlParameter _parameter = _sqlCommand.CreateParameter();
                    _parameter.DbType = DbType.Int32;
                    _parameter.ParameterName = "@ParamRoleID";
                    _parameter.Value = r.RoleID;
                    _sqlCommand.Parameters.Add(_parameter);

                    con.Open();
                    _sqlCommand.ExecuteNonQuery();   // calls the sp                 
                    con.Close();
                }
            }
        }

        public void UpdateRole(Role r)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spUpdateRole", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;

                    SqlParameter _paramRoleName = _sqlCommand.CreateParameter();
                    _paramRoleName.DbType = DbType.String;
                    _paramRoleName.ParameterName = "@ParamRoleName";
                    _paramRoleName.Value = r.RoleName;
                    _sqlCommand.Parameters.Add(_paramRoleName);

                    SqlParameter _paramRoleID = _sqlCommand.CreateParameter();
                    _paramRoleID.DbType = DbType.Int32;
                    _paramRoleID.ParameterName = "@ParamRoleID";
                    _paramRoleID.Value = r.RoleID;
                    _sqlCommand.Parameters.Add(_paramRoleID);

                    con.Open();
                    _sqlCommand.ExecuteNonQuery();   // calls the sp                 
                    con.Close();
                }
            }
        }
        #endregion

        #region User CRUD
        public List<User> GetUsers()
        {
            List<User> _list = new List<User>();

            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spGetUser", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;

                    con.Open();
                    User _user;

                    using (SqlDataReader reader = _sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _user = new User
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                LastName = (string)reader["LastName"],
                                FirstName = (string)reader["FirstName"],
                                UserName = (string)reader["UserName"],
                                Password = (string)reader["Password"],
                                RoleID_FK = reader.GetInt32(reader.GetOrdinal("RoleID")),
                                Role = new Role
                                {
                                    RoleID = reader.GetInt32(reader.GetOrdinal("RoleID")),
                                    RoleName = (string)reader["RoleName"]
                                }
                            };
                            _list.Add(_user);
                        }
                    }
                    con.Close();
                }
            }
            return _list;
        }

        public void CreateUser(User u)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spCreateUser", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;

                    SqlParameter _paramFirstName = _sqlCommand.CreateParameter();
                    _paramFirstName.DbType = DbType.String;
                    _paramFirstName.ParameterName = "@ParamFirstName";
                    _paramFirstName.Value = u.FirstName;
                    _sqlCommand.Parameters.Add(_paramFirstName);

                    SqlParameter _paramLastName = _sqlCommand.CreateParameter();
                    _paramLastName.DbType = DbType.String;
                    _paramLastName.ParameterName = "@ParamLastName";
                    _paramLastName.Value = u.LastName;
                    _sqlCommand.Parameters.Add(_paramLastName);

                    SqlParameter _paramUserName = _sqlCommand.CreateParameter();
                    _paramUserName.DbType = DbType.String;
                    _paramUserName.ParameterName = "@ParamUserName";
                    _paramUserName.Value = u.UserName;
                    _sqlCommand.Parameters.Add(_paramUserName);

                    SqlParameter _paramPassword = _sqlCommand.CreateParameter();
                    _paramPassword.DbType = DbType.String;
                    _paramPassword.ParameterName = "@ParamPassword";
                    _paramPassword.Value = u.Password;
                    _sqlCommand.Parameters.Add(_paramPassword);

                    SqlParameter _paramRoleIdFK = _sqlCommand.CreateParameter();
                    _paramRoleIdFK.DbType = DbType.Int32;
                    _paramRoleIdFK.ParameterName = "@ParamRoleID";
                    _paramRoleIdFK.Value = u.RoleID_FK;
                    _sqlCommand.Parameters.Add(_paramRoleIdFK);

                    //SqlParameter _paramAuthorIDReturn = _sqlCommand.CreateParameter();
                    //_paramAuthorIDReturn.DbType = DbType.Int32;
                    //_paramAuthorIDReturn.ParameterName = "@ParamOutAuthorID";
                    //var pk = _sqlCommand.Parameters.Add(_paramAuthorIDReturn);
                    //_paramAuthorIDReturn.Direction = ParameterDirection.Output;

                    con.Open();
                    _sqlCommand.ExecuteNonQuery();   // calls the sp 
                                                     //var result = _paramAuthorIDReturn.Value;
                    con.Close();
                    //return (int)result;


                }
            }
        }     

        public void UpdateUser(User u)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spUpdateUser", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;


                    SqlParameter _paramUserID = _sqlCommand.CreateParameter();
                    _paramUserID.DbType = DbType.Int32;
                    _paramUserID.ParameterName = "@ParamUserID";
                    _paramUserID.Value = u.UserID;
                    _sqlCommand.Parameters.Add(_paramUserID);


                    SqlParameter _paramFirstName = _sqlCommand.CreateParameter();
                    _paramFirstName.DbType = DbType.String;
                    _paramFirstName.ParameterName = "@ParamFirstName";
                    _paramFirstName.Value = u.FirstName;
                    _sqlCommand.Parameters.Add(_paramFirstName);


                    SqlParameter _paramLastName = _sqlCommand.CreateParameter();
                    _paramLastName.DbType = DbType.String;
                    _paramLastName.ParameterName = "@ParamLastName";
                    _paramLastName.Value = u.LastName;
                    _sqlCommand.Parameters.Add(_paramLastName);


                    SqlParameter _paramUserName = _sqlCommand.CreateParameter();
                    _paramUserName.DbType = DbType.String;
                    _paramUserName.ParameterName = "@ParamUserName";
                    _paramUserName.Value = u.UserName;
                    _sqlCommand.Parameters.Add(_paramUserName);


                    SqlParameter _paramPassword = _sqlCommand.CreateParameter();
                    _paramPassword.DbType = DbType.String;
                    _paramPassword.ParameterName = "@ParamPassword";
                    _paramPassword.Value = u.Password;
                    _sqlCommand.Parameters.Add(_paramPassword);


                    SqlParameter _paramRoleID = _sqlCommand.CreateParameter();
                    _paramRoleID.DbType = DbType.Int32;
                    _paramRoleID.ParameterName = "@ParamRoleID";
                    _paramRoleID.Value = u.RoleID_FK;
                    _sqlCommand.Parameters.Add(_paramRoleID);


                    con.Open();
                    _sqlCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public void DeleteUser(User u)


        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spDeleteUser", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;


                    SqlParameter _parameter = _sqlCommand.CreateParameter();
                    _parameter.DbType = DbType.Int32;
                    _parameter.ParameterName = "@ParamUserID";
                    _parameter.Value = u.UserID;
                    _sqlCommand.Parameters.Add(_parameter);


                    con.Open();
                    _sqlCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        #endregion User CRUD

        #region Author CRUD

        public List<Author> GetAuthor()
        {
            List<Author> _listAuth = new List<Author>();

            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spGetAuthor", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;
                    //_sqlCommand.Parameters.AddWithValue("@BookID", inOneParticularBook);

                    con.Open();
                    Author _Author;

                    using (SqlDataReader reader = _sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _Author = new Author
                            {

                                AuthorID = reader.GetInt32(reader.GetOrdinal("AuthorID")),
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                Bio = (string)reader["Bio"],
                                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                BirthLocation = (string)reader["BirthLocation"]

                            };
                            _listAuth.Add(_Author);
                        }
                    }
                    con.Close();
                }
                return _listAuth;
            }

        }

        public void CreateAuthor(Author a)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spCreateAuthor", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;
                    //_sqlCommand.Parameters.AddWithValue("@ParamRoleName", r.RoleName);
                    //_sqlCommand.Parameters.Add("@ParamRoleName", SqlDbType.NVarChar(100)).Value = r.RoleName;
                    SqlParameter _paramFirstName = _sqlCommand.CreateParameter();
                    _paramFirstName.DbType = DbType.String;
                    _paramFirstName.ParameterName = "@ParamFirstName";
                    _paramFirstName.Value = a.FirstName;
                    _sqlCommand.Parameters.Add(_paramFirstName);

                    SqlParameter _paramLastName = _sqlCommand.CreateParameter();
                    _paramLastName.DbType = DbType.String;
                    _paramLastName.ParameterName = "@ParamLastName";
                    _paramLastName.Value = a.LastName;
                    _sqlCommand.Parameters.Add(_paramLastName);

                    SqlParameter _paramBio = _sqlCommand.CreateParameter();
                    _paramBio.DbType = DbType.String;
                    _paramBio.ParameterName = "@ParamBio";
                    _paramBio.Value = a.Bio;
                    _sqlCommand.Parameters.Add(_paramBio);

                    SqlParameter _paramDateOfBirth = _sqlCommand.CreateParameter();
                    _paramDateOfBirth.DbType = DbType.DateTime;
                    _paramDateOfBirth.ParameterName = "@ParamDateOfBirth";
                    _paramDateOfBirth.Value = a.DateOfBirth;
                    _sqlCommand.Parameters.Add(_paramDateOfBirth);

                    SqlParameter _paramBirthLocation = _sqlCommand.CreateParameter();
                    _paramBirthLocation.DbType = DbType.String;
                    _paramBirthLocation.ParameterName = "@ParamBirthLocation";
                    _paramBirthLocation.Value = a.BirthLocation;
                    _sqlCommand.Parameters.Add(_paramBirthLocation);


                    //SqlParameter _paramAuthorIDReturn = _sqlCommand.CreateParameter();
                    //_paramAuthorIDReturn.DbType = DbType.Int32;
                    //_paramAuthorIDReturn.ParameterName = "@ParamOutAuthorID";
                    //var pk = _sqlCommand.Parameters.Add(_paramAuthorIDReturn);
                    //_paramAuthorIDReturn.Direction = ParameterDirection.Output;

                    con.Open();
                    _sqlCommand.ExecuteNonQuery();   // calls the sp 
                                                     //var result = _paramAuthorIDReturn.Value;
                    con.Close();
                    //return (int)result;
                }
            }
        }

        public void UpdateAuthor(Author a)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spUpdateAuthor", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;


                    SqlParameter _paramFirstName = _sqlCommand.CreateParameter();
                    _paramFirstName.DbType = DbType.String;
                    _paramFirstName.ParameterName = "@ParamFirstName";
                    _paramFirstName.Value = a.FirstName;
                    _sqlCommand.Parameters.Add(_paramFirstName);

                    SqlParameter _paramLastName = _sqlCommand.CreateParameter();
                    _paramLastName.DbType = DbType.String;
                    _paramLastName.ParameterName = "@ParamLastName";
                    _paramLastName.Value = a.LastName;
                    _sqlCommand.Parameters.Add(_paramLastName);

                    SqlParameter _paramBio = _sqlCommand.CreateParameter();
                    _paramBio.DbType = DbType.String;
                    _paramBio.ParameterName = "@ParamBio";
                    _paramBio.Value = a.Bio;
                    _sqlCommand.Parameters.Add(_paramBio);

                    SqlParameter _paramDateOfBirth = _sqlCommand.CreateParameter();
                    _paramDateOfBirth.DbType = DbType.DateTime;
                    _paramDateOfBirth.ParameterName = "@ParamDateOfBirth";
                    _paramDateOfBirth.Value = a.DateOfBirth;
                    _sqlCommand.Parameters.Add(_paramDateOfBirth);

                    SqlParameter _paramBirthLocation = _sqlCommand.CreateParameter();
                    _paramBirthLocation.DbType = DbType.String;
                    _paramBirthLocation.ParameterName = "@ParamBirthLocation";
                    _paramBirthLocation.Value = a.BirthLocation;
                    _sqlCommand.Parameters.Add(_paramBirthLocation);

                    SqlParameter _paramAuthorID = _sqlCommand.CreateParameter();
                    _paramAuthorID.DbType = DbType.Int32;
                    _paramAuthorID.ParameterName = "@ParamAuthorID";
                    _paramAuthorID.Value = a.AuthorID;
                    _sqlCommand.Parameters.Add(_paramAuthorID);

                    con.Open();
                    _sqlCommand.ExecuteNonQuery();   // calls the sp                 
                    con.Close();
                }
            }
        }

        public void DeleteAuthor(Author a)

        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spDeleteAuthor", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;

                    SqlParameter _parameter = _sqlCommand.CreateParameter();
                    _parameter.DbType = DbType.Int32;
                    _parameter.ParameterName = "@ParamAuthorID";
                    _parameter.Value = a.AuthorID;
                    _sqlCommand.Parameters.Add(_parameter);

                    con.Open();
                    _sqlCommand.ExecuteNonQuery();   // calls the sp                 
                    con.Close();
                }
            }
        }

        #endregion

        #region Book CRUD

        public List<Book> GetBooks()
        {

            // TESTing - just mock the list right now
            // MockBooks _mockBooks = new MockBooks();
            // return _mockBooks.Books;

            // implement this with ado.net  
            List<Book> _list = new List<Book>();
            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spGetBook", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;

                    con.Open();
                    Book _book;
                    using (SqlDataReader reader = _sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _book = new Book
                            {
                                BookID = reader.GetInt32(reader.GetOrdinal("BookID")),
                                Title = (string)reader["Title"],
                                Description = (string)reader["Description"],
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                IsPaperback =  reader.GetBoolean(reader.GetOrdinal("IsPaperBack")),
                                PublishDate = reader.GetDateTime(reader.GetOrdinal("PublishDate")),
                                AuthorID_FK = reader.GetInt32(reader.GetOrdinal("AuthorID_FK")),
                                GenreID_FK = reader.GetInt32(reader.GetOrdinal("GenreID_FK")),
                                PublisherID_FK = reader.GetInt32(reader.GetOrdinal("PublisherID_FK"))
                            };
                            _list.Add(_book);
                        }
                    }
                    con.Close();
                }
            }
            return _list;
        }

        public int CreateBook(Book b)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spCreateBook", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;

                    SqlParameter _paramTitle = _sqlCommand.CreateParameter();
                    _paramTitle.DbType = DbType.String;
                    _paramTitle.ParameterName = "@ParamTitle";
                    _paramTitle.Value = b.Title;
                    _sqlCommand.Parameters.Add(_paramTitle);

                    SqlParameter _paramDescription = _sqlCommand.CreateParameter();
                    _paramDescription.DbType = DbType.String;
                    _paramDescription.ParameterName = "@ParamDescription";
                    _paramDescription.Value = b.Description;
                    _sqlCommand.Parameters.Add(_paramDescription);

                    SqlParameter _paramPrice = _sqlCommand.CreateParameter();
                    _paramPrice.DbType = DbType.Decimal;
                    _paramPrice.ParameterName = "@ParamPrice";
                    _paramPrice.Value = b.Price;
                    _sqlCommand.Parameters.Add(_paramPrice);

                    SqlParameter _paramIsPaperBack = _sqlCommand.CreateParameter();
                    _paramIsPaperBack.DbType = DbType.Boolean;
                    _paramIsPaperBack.ParameterName = "@ParamIsPaperBack";
                    _paramIsPaperBack.Value = b.IsPaperback;
                    _sqlCommand.Parameters.Add(_paramIsPaperBack);

                    SqlParameter _paramPublishDate = _sqlCommand.CreateParameter();
                    _paramPublishDate.DbType = DbType.DateTime;
                    _paramPublishDate.ParameterName = "@ParamPublishDate";
                    _paramPublishDate.Value = b.PublishDate;
                    _sqlCommand.Parameters.Add(_paramPublishDate);

                    SqlParameter _paramGenreIDFK = _sqlCommand.CreateParameter();
                    _paramGenreIDFK.DbType = DbType.Int32;
                    _paramGenreIDFK.ParameterName = "@ParamGenreID_FK";
                    _paramGenreIDFK.Value = b.GenreID_FK;
                    _sqlCommand.Parameters.Add(_paramGenreIDFK);

                    SqlParameter _paramAuthorIDFK = _sqlCommand.CreateParameter();
                    _paramAuthorIDFK.DbType = DbType.Int32;
                    _paramAuthorIDFK.ParameterName = "@ParamAuthorID_FK";
                    _paramAuthorIDFK.Value = b.AuthorID_FK;
                    _sqlCommand.Parameters.Add(_paramAuthorIDFK);

                    SqlParameter _paramPublisherIDFK = _sqlCommand.CreateParameter();
                    _paramPublisherIDFK.DbType = DbType.Int32;
                    _paramPublisherIDFK.ParameterName = "@ParamPublisherID_FK";
                    _paramPublisherIDFK.Value = b.PublisherID_FK;
                    _sqlCommand.Parameters.Add(_paramPublisherIDFK);

                    SqlParameter _paramBookIDReturn = _sqlCommand.CreateParameter();
                    _paramBookIDReturn.DbType = DbType.Int32;
                    _paramBookIDReturn.ParameterName = "@ParamOutBookID";
                    _sqlCommand.Parameters.Add(_paramBookIDReturn);
                    _paramBookIDReturn.Direction = ParameterDirection.Output;

                    con.Open();
                    _sqlCommand.ExecuteNonQuery();   // calls the sp                                                      
                    var result = _paramBookIDReturn.Value;
                    con.Close();
                    return (int)result;
                }

            }
        }

        public void UpdateBook(Book b)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spUpdateBook", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;

                    SqlParameter _paramBookID = _sqlCommand.CreateParameter();
                    _paramBookID.DbType = DbType.Int32;
                    _paramBookID.ParameterName = "@ParamBookID";
                    _paramBookID.Value = b.BookID;
                    _sqlCommand.Parameters.Add(_paramBookID);

                    SqlParameter _paramTitle = _sqlCommand.CreateParameter();
                    _paramTitle.DbType = DbType.String;
                    _paramTitle.ParameterName = "@ParamTitle";
                    _paramTitle.Value = b.Title;
                    _sqlCommand.Parameters.Add(_paramTitle);

                    SqlParameter _paramDescription = _sqlCommand.CreateParameter();
                    _paramDescription.DbType = DbType.String;
                    _paramDescription.ParameterName = "@ParamDescription";
                    _paramDescription.Value = b.Description;
                    _sqlCommand.Parameters.Add(_paramDescription);

                    SqlParameter _paramPrice = _sqlCommand.CreateParameter();
                    _paramPrice.DbType = DbType.Decimal;
                    _paramPrice.ParameterName = "@ParamPrice";
                    _paramPrice.Value = b.Price;
                    _sqlCommand.Parameters.Add(_paramPrice);

                    SqlParameter _paramIsPaperBack = _sqlCommand.CreateParameter();
                    _paramIsPaperBack.DbType = DbType.Boolean;
                    _paramIsPaperBack.ParameterName = "@ParamIsPaperBack";
                    _paramIsPaperBack.Value = b.IsPaperback;
                    _sqlCommand.Parameters.Add(_paramIsPaperBack);

                    SqlParameter _paramPublishDate = _sqlCommand.CreateParameter();
                    _paramPublishDate.DbType = DbType.DateTime;
                    _paramPublishDate.ParameterName = "@ParamPublishDate";
                    _paramPublishDate.Value = b.PublishDate;
                    _sqlCommand.Parameters.Add(_paramPublishDate);

                    SqlParameter _paramGenreIDFK = _sqlCommand.CreateParameter();
                    _paramGenreIDFK.DbType = DbType.Int32;
                    _paramGenreIDFK.ParameterName = "@ParamGenreID_FK";
                    _paramGenreIDFK.Value = b.GenreID_FK;
                    _sqlCommand.Parameters.Add(_paramGenreIDFK);

                    SqlParameter _paramAuthorIDFK = _sqlCommand.CreateParameter();
                    _paramAuthorIDFK.DbType = DbType.Int32;
                    _paramAuthorIDFK.ParameterName = "@ParamAuthorID_FK";
                    _paramAuthorIDFK.Value = b.AuthorID_FK;
                    _sqlCommand.Parameters.Add(_paramAuthorIDFK);

                    SqlParameter _paramPublisherIDFK = _sqlCommand.CreateParameter();
                    _paramPublisherIDFK.DbType = DbType.Int32;
                    _paramPublisherIDFK.ParameterName = "@ParamPublisherID_FK";
                    _paramPublisherIDFK.Value = b.PublisherID_FK;
                    _sqlCommand.Parameters.Add(_paramPublisherIDFK);
               
                    con.Open();
                    _sqlCommand.ExecuteNonQuery();   // calls the sp                                                      
                    con.Close();

                }
            }
        }

        public void DeleteBook(Book b) 
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                using (SqlCommand _sqlCommand = new SqlCommand("spDeleteBook", con))
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandTimeout = 30;

                    SqlParameter _parameter = _sqlCommand.CreateParameter();
                    _parameter.DbType = DbType.Int32;
                    _parameter.ParameterName = "@ParamBookID";
                    _parameter.Value = b.BookID;
                    _sqlCommand.Parameters.Add(_parameter);

                    con.Open();
                    _sqlCommand.ExecuteNonQuery();   // calls the sp                 
                    con.Close();
                }
            }
        }

        #endregion

    }
}