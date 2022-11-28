using BusinessObject;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessing
{
    public class RoomDAO:BaseDAL
    {
        private static RoomDAO instance = null;
        private static readonly object instanceLock = new object();
        private RoomDAO() { }
        public static RoomDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoomDAO();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<Room> GetRoomList()
        {
            IDataReader dataReader = null;
            string SQLSelect = "Select RoomNumber,RoomStatus from Room";
            var rooms = new List<Room>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    rooms.Add(new Room
                    {
                        
                        RoomNumber=dataReader.GetInt32(0),
                        RoomStatus=dataReader.GetString(1),
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return rooms;
        }

        
        public IEnumerable<Room> GetRoomListByStatus(string status)
        {
            IDataReader dataReader = null;
            string SQLSelect = "Select RoomNumber,RoomStatus from Room where RoomStatus=@RoomStatus";
            var rooms = new List<Room>();

            try
            {
                var param = dataProvider.CreateParameter("@RoomStatus", 15, status, DbType.String);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                while (dataReader.Read())
                {
                    rooms.Add(new Room
                    {
                        RoomNumber = dataReader.GetInt32(0),
                        RoomStatus = dataReader.GetString(1),
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return rooms;
        }
        
        public Room GetRoomByNumber(int roomnumber)
        {
            Room room = null;
            IDataReader dataReader = null;
            string SQLSelect = "Select RoomNumber,RoomStatus from Room where RoomNumber=@RoomNumber";
            try
            {
                var param = dataProvider.CreateParameter("@RoomNumber", 4, roomnumber, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    room = new Room
                    {
                        RoomNumber = dataReader.GetInt32(0),
                        RoomStatus = dataReader.GetString(1),
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return room;
        }
        public void Update(Room room)
        {
            try
            {
                Room pro = GetRoomByNumber(room.RoomNumber);
                if (pro != null)
                {
                    string SQLUpdate = "Update Room set RoomNumber=@RoomNumber,RoomStatus=@RoomStatus where RoomNumber=@RoomNumber ";
                    var parameters = new List<SqlParameter>();
         
                    parameters.Add(dataProvider.CreateParameter("@RoomNumber", 4, room.RoomNumber, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@RoomStatus", 15, room.RoomStatus, DbType.String));
                    dataProvider.Update(SQLUpdate, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("Room does not already exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<int> GetRoomNumbers()
        {
            IDataReader dataReader = null;
            string SQLSelect = "Select RoomNumber from Room ";
            var rooms = new List<int>();

            try
            {
                
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    rooms.Add(dataReader.GetInt32(0));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return rooms;
        }
        public void Addnew(Room room)
        {
            try
            {
                Room pro = GetRoomByNumber(room.RoomNumber);
                if (pro == null)
                {
                    string SQLInsert = "Insert Room values(@RoomNumber, @RoomStatus)";
                    var parameters = new List<SqlParameter>();

                    parameters.Add(dataProvider.CreateParameter("@RoomNumber", 4, room.RoomNumber, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@RoomStatus", 15, room.RoomStatus, DbType.String));
                    dataProvider.Insert(SQLInsert, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("Room number already existed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        public void Remove(int roomNumber)
        {
            try
            {
                Room pro = GetRoomByNumber(roomNumber);
                if (pro != null)
                {
                    string SQLUpdate = "Delete Room where RoomNumber = @roomNumber";
                    var parameters = new List<SqlParameter>();

                    parameters.Add(dataProvider.CreateParameter("@roomNumber", 4, roomNumber, DbType.Int32));
                    
                    dataProvider.Update(SQLUpdate, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("Room number does not existed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
