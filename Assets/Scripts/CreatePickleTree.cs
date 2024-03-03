using UnityEngine;

public class CreatePickleTree : MonoBehaviour
{
    [SerializeField] private GameObject treePrefab;
    private Rigidbody rb;
    private float treeSizeMultiplier;
    private bool canPlantTree = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Invoke("CanPlantTree", 0.5f);
    }

    void Update()
    {
        if (canPlantTree)
        {
            PlantTree();
        }
    }

    private void PlantTree()
    {
        if (rb.velocity.x <= 0.1f && rb.velocity.y <= 0.1f && rb.velocity.z <= 0.1f) {
            var tree = Instantiate(treePrefab, new Vector3(transform.position.x, transform.position.y + treeSizeMultiplier*treeSizeMultiplier, transform.position.z), new Quaternion(0, 0, 0, 0));
            tree.GetComponent<TreeIncomScore>().setScoreMultiplier(treeSizeMultiplier);
            tree.transform.localScale *= treeSizeMultiplier;
            tree.transform.GetChild(0).transform.localScale = new Vector3(tree.transform.GetChild(0).transform.localScale.x + treeSizeMultiplier * treeSizeMultiplier, tree.transform.GetChild(0).transform.localScale.y, tree.transform.GetChild(0).transform.localScale.z + treeSizeMultiplier * treeSizeMultiplier);
            tree.transform.GetChild(1).transform.localScale *= treeSizeMultiplier * treeSizeMultiplier;
            Destroy(gameObject);
        }
    }

    public void setTreeSizeMultiplier(float multiplier)
    {
        treeSizeMultiplier = multiplier;
    }

    private void CanPlantTree()
    {
        canPlantTree = true;
    }
}
