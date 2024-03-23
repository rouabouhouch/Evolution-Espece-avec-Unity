using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddFoodOnStart : MonoBehaviour
{
    public string resourceName = "Nourriture"; // Nom du prefab � charger depuis le dossier "Resources"
    public static int nourrituresSpawned = 0; // Variable statique pour compter les nourritures spawn�es
    public static List<NourritureInfo> spawnedFoodList = new List<NourritureInfo>(); // Liste pour stocker les infos des nourritures spawn�es

    private int maxNourritures; // Variable pour stocker le nombre maximum de nourritures

    void Start()
    {
        // Cr�e un objet vide "food_despenser"
        GameObject foodDespenser = new GameObject("food_despenser");

        // Ajoute l'objet "food_despenser" � la sc�ne
        Instantiate(foodDespenser);

        // Trouve l'objet "Lieu_de_spawn_nourriture" dans la sc�ne
        GameObject lieuDeSpawnNourriture = GameObject.Find("Lieu_de_spawn_nourriture");

        if (lieuDeSpawnNourriture != null)
        {
            // Loop through each child of "Lieu_de_spawn_nourriture"
            foreach (Transform child in lieuDeSpawnNourriture.transform)
            {
                // Obtient les coordonn�es de position du child
                Vector3 childPosition = child.position;

              

                // Charge le prefab depuis le dossier "Resources"
                GameObject foodPrefab = Resources.Load<GameObject>(resourceName);

                if (foodPrefab != null)
                {
                    // Instancie l'objet "Nourriture" aux m�mes coordonn�es que le child
                    GameObject foodInstance = Instantiate(foodPrefab, childPosition, Quaternion.identity);

                    // Place l'instance de nourriture comme enfant de l'objet "food_despenser"
                    foodInstance.transform.parent = foodDespenser.transform;

                    // R�cup�re l'ID de la nourriture
                    int foodID = ++nourrituresSpawned;

                    // Ajoute l'info de la nourriture � la liste des nourritures spawn�es
                    spawnedFoodList.Add(new NourritureInfo(foodID, true, childPosition));
                }
                else
                {
                    Debug.LogError("Le prefab '" + resourceName + "' n'a pas pu �tre charg� depuis le dossier 'Resources'. Assurez-vous que le prefab est correctement plac� dans le dossier 'Resources'.");
                }
                LogSpawnedFoodList();   
            }
        }
        else
        {
            Debug.LogError("L'objet 'Lieu_de_spawn_nourriture' n'a pas �t� trouv� dans la sc�ne. Assurez-vous que l'objet 'Lieu_de_spawn_nourriture' est pr�sent et correctement nomm� dans la sc�ne.");
        }

        // Stocke le nombre maximum de nourritures
        maxNourritures = nourrituresSpawned;

        // D�marre la coroutine pour v�rifier le respawn des nourritures
        StartCoroutine(CheckRespawn());
    }

    IEnumerator CheckRespawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Attend une seconde avant de v�rifier � nouveau

            // V�rifie si le nombre de nourritures spawn�es a diminu�
            if (nourrituresSpawned < maxNourritures)
            {
                // D�marre le timer pour le respawn apr�s 20 secondes
                StartCoroutine(RespawnTimer());
            }
        }
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(20f); // Attend 20 secondes avant de respawn

        // Recherche une nourriture non spawn�e dans la liste des nourritures
        NourritureInfo missingFood = spawnedFoodList.Find(food => !food.isSpawned);

        if (missingFood != null)
        {
            // Charge le prefab depuis le dossier "Resources"
            GameObject foodPrefab = Resources.Load<GameObject>(resourceName);

            if (foodPrefab != null)
            {
                // Instancie l'objet "Nourriture" � la position enregistr�e
                GameObject foodInstance = Instantiate(foodPrefab, missingFood.position, Quaternion.identity);

                // Incr�mente le nombre de nourritures spawn�es
                nourrituresSpawned++;

                // Marque la nourriture comme spawn�e
                missingFood.isSpawned = true;

                // Log the ID table with each ID and spawn boolean
                LogSpawnedFoodList();
            }
            else
            {
                Debug.LogError("Le prefab '" + resourceName + "' n'a pas pu �tre charg� depuis le dossier 'Resources'. Assurez-vous que le prefab est correctement plac� dans le dossier 'Resources'.");
            }
        }
    }

       void LogSpawnedFoodList()
    {
        Debug.Log("Spawned Food List:");
        foreach (var foodInfo in spawnedFoodList)
        {
            Debug.Log("ID: " + foodInfo.id + ", Spawned: " + foodInfo.isSpawned + ", Position: " + foodInfo.position);
        }
    }
}

 