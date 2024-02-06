using UnityEngine;

public class OldRuneImporter : MonoBehaviour
{
    [SerializeField] private OldRunesStorage runesStorage;


    public void ImportNew()
    {
        Texture2D[] imagesToImport = Resources.LoadAll<Texture2D>("Runes/Blured");
        foreach (Texture2D imageToImport in imagesToImport)
        {
            if (runesStorage.runeImages.ContainsKey(imageToImport.name)) continue;

            RuneImage runeImageToSave = ImageToRuneImage(imageToImport);
            runesStorage.SaveNewRuneImage(runeImageToSave);
        }
    }

    public void ReimportAll()
    {
        Texture2D[] imagesToImport = Resources.LoadAll<Texture2D>("Runes/Blured");
        foreach (Texture2D imageToImport in imagesToImport)
        {
            RuneImage runeImageToSave = ImageToRuneImage(imageToImport);
            runesStorage.SaveNewRuneImage(runeImageToSave);
        }
    }


    private RuneImage ImageToRuneImage(Texture2D imageToConvert)
    {
        RuneImage runeImage = new(imageToConvert.name);

        Vector2 momentsSum = new();
        for (int y = 0; y < RuneImage.SIZE_IN_PIXELS; y++)
        {
            for (int x = 0; x < RuneImage.SIZE_IN_PIXELS; x++)
            {
                byte pixel = (byte)(imageToConvert.GetPixel(x, y).r * -255 + 255);
                runeImage.image[x, y] = pixel;
                runeImage.mass += pixel;
                momentsSum += pixel * new Vector2(x, y);
            }
        }
        runeImage.massCenter = momentsSum / runeImage.mass;

        return runeImage;
    }
}