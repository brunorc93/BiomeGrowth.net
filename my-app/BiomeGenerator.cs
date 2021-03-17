using System.Collections.Generic;
using System.IO;
using static System.Math;
using static Structs;
using static Helper;
using System;
using System.Drawing;
using System.Windows.Forms;

public class BiomeGenerator
{
    // vars
      //---------- common vars --------public----
        public bool debugging = true;
        public bool useSeed = false;
        public int seed = 42;
        public bool showcase = false;
        public bool _save = true;
        public bool finished = false;
        public int auxValue = 0;
        public int generation_phase = 0;
        public int roundNumber = 0;
        public int size = 512;
        public int updateCount = 0;
        public Color[,] colors; // indexed as y,x
        private string[] nodes;
        private int[] nodesRed = null;
        private int[] nodesArea = null;
        private List<Vector2Int> nodesV2 = new List<Vector2Int>();
        private List<List<Vector2Int>> nodesV2List = new List<List<Vector2Int>>();
        private Vector2[] perlinPoint;
        private float[][,] gradients; // indexed as y,x
        public Form form;
        private Random rnd = new Random();
        public bool continuous_save = false;
        public Perlin perlin = new Perlin();

// main ---------------------------------------

    void _Start()
    { 
      if (useSeed) { rnd = new Random(seed); }
      
      colors = new Color[size,size];
      for (int i=0; i<size; i++){ for (int j=0; j<size; j++) {
        colors[j,i] = Color.FromArgb(255,0,0,0);
      }}
    }

    public void Update()
    {
      if (!finished)
      {
        updateCount++;
        switch (generation_phase)
        {
          case 0: // Creating TG
            CallDebug("Starting Biome Generator");
            _Start();
            generation_phase++;
            break;
          case 1:  // Adding nodes
            CallDebug("Choosing Biome Nodes from list of biomes");
            AddNodeNames();
            generation_phase++;
            break;
          case 2:  // Placing nodes in island
            CallDebug("Placing node starting points");
            PlaceNodes();
            generation_phase++;
            break;
          case 3:  // Expanding nodes
            if (auxValue%350 == 0)
            {
              CallDebug("Creating nodes growth walls");
              auxValue++;
              CalculatePerlinNoise();
              // ResetGradients();
            }
            else 
            {
              CallDebug("Expanding nodes"); // here - add percentage to finish
              if (Expand()) { auxValue++; }
              else 
              {
                auxValue = 0;
                generation_phase++;
            } }
            break;
          case 4: // Saving && / || Starting  nextGenerator.cs
            if (_save)
            {
              CallDebug("saving generated biome into files");
              Save();
              _save = false;
            }
            generation_phase++;
            break;
          case 5: // Finished
            CallDebug("Finished BiomeGenerator.cs");
            finished = true;
            break;
        }

        if (showcase) {
          Showcase();
        }
      }
    }

// private methods ----------------------------
//         unique -----------------------------
    void CalculatePerlinNoise()
    {
      perlinPoint = new Vector2[nodes.Length];
      gradients = new float[nodes.Length][,];
      for (int a=0; a<nodes.Length; a++)
      {
        perlinPoint[a] = new Vector2((float)Pow(-1,rnd.Next(1,2))*500f*(float)rnd.NextDouble(),(float)Pow(-1,rnd.Next(1,2))*500f*(float)rnd.NextDouble());
        gradients [a] = new float[size,size];
        float maxValue = float.MinValue;
        float minValue = float.MaxValue;
        for (int i=0; i<size; i++)
        {
          for (int j=0; j<size; j++)
          {
            float x = (float)i*0.39f*0.39f;
            float y = (float)j*0.39f*0.39f;
            float newValue = Abs(perlin.Noise(x+perlinPoint[a].x,y+perlinPoint[a].y));
            float m = 1f;
            for (int k=0; k<5; k++)
            {
              x /= 2f;
              y /= 2f;
              m *= 2.1f;
              newValue+= m* Abs(perlin.Noise(x+perlinPoint[a].x,y+perlinPoint[a].y));
            }
            gradients[a][j,i]=newValue;
            if (newValue>maxValue) { maxValue = newValue; }
            if (newValue<minValue) { minValue = newValue; }
        } }
        for (int i=0; i<size; i++)
        {
          for (int j=0; j<size; j++)
          {
            float result = (gradients[a][j,i]-minValue)/(maxValue-minValue);
            result = result - result%0.05f;
            if (result >= 1-7*0.05f) { result = 0.99f; }
            if (result <= 7*0.05f) { result = 0f; }
            gradients[a][j,i] = result;
      } } }
    }
    void AddNodeNames()
    {
      int numberOfInlandNodes = rnd.Next(12,18);
      nodes = new string[numberOfInlandNodes];
      for (int i=1; i<=numberOfInlandNodes; i++)
      {
        nodes[i-1] = ("node_"+i.ToString("D2"));
      }
    }
    bool Expand()
    { // returns if it still has nodes to expand
      bool expanding = true;
      List<Vector2Int> auxList = new List<Vector2Int>();
      int minArea = int.MaxValue;
      int maxArea = int.MinValue;
      for (int i=0; i<nodesArea.Length; i++)
      {
        if (nodesArea[i]<minArea) { minArea = nodesArea[i]; }
        if (nodesArea[i]>maxArea) { maxArea = nodesArea[i]; }
      }
      foreach (Vector2Int point in nodesV2)
      {
        Vector2Int[] neighbours = point.Neighbours(size);
        Color nodeColor = colors[point.y,point.x];
        int index = System.Array.IndexOf(nodesRed,nodeColor.R);
        bool freeNeighboursExist = false;
        float minRnd = 0.4f;
        foreach(Vector2Int neighbour in neighbours)
        {
          Color neighbourColor = colors[neighbour.y,neighbour.x];
          if (neighbourColor.A==254) { minRnd /=6f; } 
        }
        int failCount = 0;
        foreach(Vector2Int neighbour in neighbours)
        {
          Color newColor = colors[neighbour.y,neighbour.x];
          if (newColor.A>253)
          {
            freeNeighboursExist = true;
            if (nodesArea[index]<maxArea+25)
            {
              float rnd_num = (float)rnd.NextDouble();
              int gradIndex = System.Array.IndexOf(nodesRed,nodeColor.R);
              float gradValue = gradients[gradIndex][neighbour.y,neighbour.x];
              if (rnd_num>1-minRnd-gradValue*0.5f)
              {
                newColor = Color.FromArgb(253,nodeColor.R,nodeColor.G,nodeColor.B);
                int insertIndex = rnd.Next(0,Max(auxList.Count-1,0));
                auxList.Insert(insertIndex,neighbour);
                colors[neighbour.y,neighbour.x] = newColor;
                nodesArea[index]++;
                nodesV2List[index].Add(neighbour);
              } else 
              {
                newColor = Color.FromArgb(254,newColor.R,newColor.G,newColor.B);
                failCount++;
                colors[neighbour.y,neighbour.x] = newColor;
        } } } }
        if (failCount>4)
        {
          minRnd = 0.9f;
          foreach (Vector2Int neighbour in neighbours)
          {
            float rnd_num = (float)rnd.NextDouble();
            if (rnd_num > 1-minRnd)
            {
              Color neighbourColor = colors[neighbour.y,neighbour.x] ;
              if (neighbourColor.A==254)
              {
                neighbourColor = Color.FromArgb(255,neighbourColor.R,neighbourColor.G,neighbourColor.B);
                minRnd-=0.15f;
                colors[neighbour.y,neighbour.x] = neighbourColor;
        } } } }
        if (freeNeighboursExist)
        {
          int insertIndex = rnd.Next(0,Max(auxList.Count-1,0));
          auxList.Insert(insertIndex,point);
      } }
      nodesV2 = auxList;
      if (nodesV2.Count == 0) { expanding = false; }
      return expanding;
    }
    void PlaceNodes()
    {
      int minusValue = Convert.ToInt32(Floor(255f/(nodes.Length+1f)));
      float iterN = 0;
      if (nodesArea == null) { nodesArea = new int[nodes.Length]; }
      if (nodesRed == null) { nodesRed = new int[nodes.Length]; }
      int red = 255;
      int green = rnd.Next(0,255);
      int blue = rnd.Next(0,255);
      for (int i=0; i<nodes.Length; i++)
      { 
        float minDistance = (float)(size/2f);
        bool found = false;
        Vector2Int point = new Vector2Int(0,0);
        while (!found)
        {
          iterN++;
          point = new Vector2Int(rnd.Next(0,size-1),rnd.Next(0,size-1));
          Color nodeColor = colors[point.y,point.x];
          if (nodeColor.A == 255)
          {
            float cDistance = point.DistToClosest(nodesV2);
            if (cDistance>minDistance) { found = true; iterN = 0; }
          }
          if (iterN>500) { iterN = 0; minDistance--; }
        }
        nodesV2.Add(point);
        nodesArea[i]++;
        nodesRed[i]+=red;
        nodesV2List.Add(new List<Vector2Int>());
        nodesV2List[i].Add(point);
        colors[point.y,point.x] = Color.FromArgb(253,red,green,blue);
        red-=minusValue;
        green = rnd.Next(0,255);
        blue = rnd.Next(0,255);
    } }
//         common ----------- // here - normalize-these--
    void CallDebug(string text)
    {
      if (debugging) { Console.WriteLine(updateCount.ToString("D5")+" BG"+generation_phase.ToString("D3")+"."+auxValue.ToString("D4")+"."+roundNumber.ToString("D2")+", "+text); }
    }
    Color[,] Load()
    {
      Color[,] col = new Color[10,10];
      return col;
    }
    void Save()
    {
      int n = 0;
      string path = Directory.GetCurrentDirectory()+"/Data/Saved/";
      Directory.CreateDirectory(@path);
      path += "GeneratedCount.txt";
      if (!File.Exists(path)) { File.WriteAllText(path,""); } 
      else 
      {
        string dataRead = File.ReadAllText(path);
        string[] dataLines = dataRead.Split('\n');
        n = dataLines.Length-1;
      }

      string text = "";
      foreach(string str in nodes) { text+=str; text+=";"; }
      text+="\n";
      File.AppendAllText(path,text);

      Bitmap bmp = new Bitmap(size,size);
      for (int i = 0; i<size; i++) { for (int j = 0; j<size; j++) {
        Color col = colors[j,i];
        bmp.SetPixel(i,j, col);
      } }
      path = Directory.GetCurrentDirectory() + "/Data/Saved/_png/";
      Directory.CreateDirectory(@path);
      path += n.ToString("D3")+"_.png";
      bmp.Save(path, System.Drawing.Imaging.ImageFormat.Png);
      bmp.Dispose();
    }
    void Showcase()
    {
      Bitmap bmp = new Bitmap(size,size);
      Bitmap bmp_to_save = new Bitmap(size,size);
      for (int i = 0; i<size; i++) { for (int j = 0; j<size; j++) {
        Color col = colors[j,i];
        bmp.SetPixel(i,j, col);
        bmp_to_save.SetPixel(i,j, col);
      }}
      form.BackgroundImage = bmp;
      form.BackgroundImageLayout = ImageLayout.Stretch;

      if (continuous_save) {
        int n = 0;
        
        string path = Directory.GetCurrentDirectory()+"/Data/Saved/";
        Directory.CreateDirectory(@path);
        path += "GeneratedCount.txt";
        if (!File.Exists(path)) { File.WriteAllText(path,""); } 
        else 
        {
          string dataRead = File.ReadAllText(path);
          string[] dataLines = dataRead.Split('\n');
          n = dataLines.Length-1;
        }
        if (!_save) {
          n--;
        }

        path = Directory.GetCurrentDirectory() + "/Data/Saved/_png/"+n.ToString("D3")+"/";
        Directory.CreateDirectory(@path);
        path += updateCount.ToString("D4")+"_.png";
        bmp_to_save.Save(path, System.Drawing.Imaging.ImageFormat.Png);
      }
    }

}