using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;

namespace Updater
{
    /// <summary>
    /// �������� - �������� ������ � ��
    /// </summary>
    public class FilesManager
    {
        private String _connectionString;

        public String LastError;

        public FilesManager(String ConnectionString)
        {
            _connectionString = ConnectionString;
        }
        #region �������� ������ � ��
        /// <summary>
        /// �������� ����� � �� 
        /// </summary>
        /// <param name="fileName">������ ���� � ��� ����� ��� �������� � ��</param>
        public void Upload(String fullFileName)
        {
            SqlConnection CN = MiscFunction.OpenConnection(_connectionString);
            RIPEMD160 myRIPEMD160 = RIPEMD160Managed.Create();
            String fileName = MiscFunction.GetFileName(fullFileName);

            FileStream fs = new System.IO.FileStream(fullFileName, FileMode.Open, FileAccess.Read);
            Byte[] imageData = new Byte[fs.Length];
            fs.Read(imageData, 0, Convert.ToInt32(fs.Length));

            //SHA-1 
            byte[] hashValue;
            hashValue = myRIPEMD160.ComputeHash(fs);         
            string result = BitConverter.ToString(hashValue).Replace("-", String.Empty);
            //System.IO.File.WriteAllText("C:\\Users\\�������\\Documents\\Uploader\\Updater\\bin\\Debug\\TestFile2.txt", result);

            fs.Close();

            String strSQL;

            if (!ChekExists(CN, fileName))
            {//INSERT
                strSQL = "INSERT INTO Srv_ProgramFile (version, name, author, binaryData, Date, hashCode) " +
                    "VALUES (1, @name, @autor, @binaryData, @Date, @hashCode)";
            }
            else
            {//UPDATE
                strSQL = "UPDATE Srv_ProgramFile SET version=version+1, name=@name, author=@autor, binaryData=@binaryData " +
                    "WHERE name=@name";
            }

            SqlCommand cmd = new SqlCommand(strSQL, CN) {CommandTimeout = 60};
            cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 128)).Value = fileName;
            cmd.Parameters.Add(new SqlParameter("@autor", SqlDbType.NVarChar, 50)).Value = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            cmd.Parameters.Add(new SqlParameter("@binaryData", SqlDbType.Image)).Value = imageData;
            cmd.Parameters.Add("@Date", DateTime.Now.Date);
            cmd.Parameters.Add("@hashCode", SqlDbType.NVarChar, 50).Value = result;
            cmd.ExecuteNonQuery();

            CN.Close();
        }

        /// <summary>
        ///�������� ������� ����� � �� ��� �����������: ��������� ���������� ��� ���������� ����� ������
        /// </summary>
        /// <param name="CN"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private Boolean ChekExists(SqlConnection CN, String fileName)
        {
            Boolean res = false;

            String strSQL = "SELECT idFile FROM Srv_ProgramFile WHERE name = '" + fileName + "'";
            SqlCommand cmd = new SqlCommand(strSQL, CN);
            SqlDataReader SDR = cmd.ExecuteReader();

            Int32 idFile = 0;

            while (SDR.Read())
            {
                idFile = Convert.ToInt32(SDR.GetValue(0));
            }

            SDR.Close();

            if (idFile != 0)
                res = true;

            return res;
        }
        #endregion
    }
}
