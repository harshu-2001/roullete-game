 
using Google;
using Firebase.Database;


public class GlobalConstants
{
    private static GoogleSignInUser _instance;
    public static GoogleSignInUser Instance 
    {
        get
        {
            // Reads are usually simple
            return _instance;
        }
        set
        {
            // You can add logic here for race conditions,
            // or other measurements
            _instance = value;
        }
    }

    private static int _coinvalue;
    public static int CoinValue{
         get
        {
            return _coinvalue;
        }
        set
        {
            _coinvalue = value;
        }
    }

    private static bool _playerExists;
    public static bool PlayerExists{
        get{
            return _playerExists;
        }

        set{
            _playerExists = value;
        }
    }

    private static DatabaseReference _dbRef;
    public  static DatabaseReference DbRef{
        get
        {
            return _dbRef;
        }
        set
        {
            _dbRef = value;
        }
    }

     private static DataToSave _dts;
    public  static DataToSave Dts{
        get
        {
            return _dts;
        }
        set
        {
            _dts = value;
        }
    }

}