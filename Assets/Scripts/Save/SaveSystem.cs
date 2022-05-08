using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
   private static string path = $"{Application.persistentDataPath}/Player.json";
   public static void SavePlayer(PlayerManager player)
   {
      BinaryFormatter formatter = new BinaryFormatter();
      
      FileStream stream = new FileStream(path, FileMode.Create);

      PlayerDataSave data = new PlayerDataSave(player);
      formatter.Serialize(stream, data);
      stream.Close();
   }

   public static PlayerDataSave LoadPlayer()
   {
      if (File.Exists(path))
      {
         BinaryFormatter formatter = new BinaryFormatter();
         FileStream stream = new FileStream(path, FileMode.Open);

         PlayerDataSave data = formatter.Deserialize(stream) as PlayerDataSave;
         stream.Close();
         
         return data;
      }
      else
      {
         Debug.LogError("pas de fichier pour load la sauvegarde");
         return null;
      }
   }
}