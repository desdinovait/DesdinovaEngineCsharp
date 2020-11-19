//Using di sistema
using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
//Using XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
//Using Desdinova Engine X
using DesdinovaModelPipeline;
using DesdinovaModelPipeline.Helpers;

namespace DesdinovaModelPipeline
{
    public struct XModelBounding
    {
        public BoundingSphere boundingSphereOriginal;
        public BoundingSphere boundingSphere;
        public bool boundingSphere_InFrustrum;
        public float boundingSphere_Distance;
    }

    public class XModel : Engine3DObject
    {
        //Nome
        private string assetName;
        public string AssetName
        {
            get { return assetName; }
        }
	
        //Modello principale
        private Model model = null;
        public Model Model
        {
            get { return model; }
        }

        //Lista dei materiali applicati (mesh x parti)
        private Material[][] materials = null;
        public Material[][] Materials
        {
            get { return materials; }
        }
	
        //Matrici assolute delle singole mesh
        private Matrix[] absoluteBoneTransforms = null;
        public Matrix[] AbsoluteBoneTransforms
        {
            get { return absoluteBoneTransforms; }
        }

        //Sfere di collisione
        private BoundingSphere[] collisionSpheresOriginal = null;
        private BoundingSphere[] collisionSpheres = null;
        public BoundingSphere[] CollisionSpheres
        {
            get { return collisionSpheres; }
        }

        //Bounding Box 
        private BoundingBox boundingBoxOriginal;
        public BoundingBox BoundingBoxOriginal
        {
            get { return boundingBoxOriginal; }
        }

        //Bounding sphere originale
        private BoundingSphere boundingSphereOriginal;
        public BoundingSphere BoundingSphereOriginal
        {
            get { return boundingSphereOriginal; }
        }

        /*Bounding spheres trasformata
        private BoundingSphere boundingSphere;
        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
        }*/

        //Bounding sphere originali e trasformate delle singole mesh
        private XModelBounding[] meshBoundingSphere;

        //Dimensioni
        private Dimensions dimensions = Dimensions.Zero;
        public Dimensions Dimension
        {
            get { return dimensions; }
        }

        //Centro del modello
        public Vector3 Center   
        {
            get { return Vector3.Transform(boundingSphereOriginal.Center, worldMatrix * this.externalMatrix); }
        }

        //Raggio totale del modello
        public float Radius
        {
            get { return boundingSphereOriginal.Radius * worldMatrix.Forward.Length(); }
        }

        //Posizione 2D
        private Vector2 position2D = Vector2.Zero;
        public Vector2 Position2D
        {
            get { return position2D; }
        }

        //Posizione
        private Vector3 position = Vector3.Zero;
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float PositionX
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public float PositionY
        {
            get { return position.Y; }
            set { position.Y = value; }
        }
        public float PositionZ
        {
            get { return position.Z; }
            set { position.Z = value; }
        }

        //Rotazione
        private Vector3 rotation = Vector3.Zero;
        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public float RotationX
        {
            get { return rotation.X; }
            set { rotation.X = value; }
        }
        public float RotationY
        {
            get { return rotation.Y; }
            set { rotation.Y = value; }
        }
        public float RotationZ
        {
            get { return rotation.Z; }
            set { rotation.Z = value; }
        }

        //Scalatura
        private Vector3 scale = Vector3.One;
        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        public float ScaleX
        {
            get { return scale.X; }
            set { scale.X = value; }
        }
        public float ScaleY
        {
            get { return scale.Y; }
            set { scale.Y = value; }
        }
        public float ScaleZ
        {
            get { return scale.Z; }
            set { scale.Z = value; }
        }

        //Matrice finale di posizione nel mondo
        private Matrix worldMatrix = Matrix.Identity;
        public Matrix WorldMatrix
        {
            get { return worldMatrix; }
        }

        //Ancora (il modello è ancorato a qualcosa)
        private Matrix externalMatrix = Matrix.Identity;
        public Matrix ExternalMatrix
        {
            get { return externalMatrix; }
            set { externalMatrix = value; }
        }
	
        //Conteggio
        private CountInfo countInfo = new CountInfo();
        public CountInfo CountInfo
        {
            get { return countInfo; }
        }

        //Nel frustrum (generale)
        private bool inFrustrum = true;
        public bool InFrustrum
        {
            get { return inFrustrum; }
        }

        //Ancoraggio
        List<XModelAnchor> anchors = new List<XModelAnchor>();
	    public List<XModelAnchor> Anchors
	    {
		    get { return anchors;}
	    }

        //Proprietà di rendering 
        private BlendMode blendProperties = BlendMode.AlphaBlend;
        public BlendMode BlendProperties
        {
            get { return blendProperties; }
            set { blendProperties = value; }
        }

        //Cast shadow
        private bool  castShadow = true;
        public bool  CastShadow
        {
            get { return castShadow; }
            set { castShadow = value; }
        }
	

        public XModel(Scene parentScene): base(parentScene)
        {

        }

        public virtual bool Initialize(string assetName)
        {
            try
            {               
                //Nome
                this.assetName = assetName;

                //Carica il modello da file
                model = base.ParentScene.SceneContent.Load<Model>(assetName);
               
                //Preleva le trasformazioni assolute
                absoluteBoneTransforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(absoluteBoneTransforms);

                //Crea le bounding generale
                boundingSphereOriginal = new BoundingSphere();
                boundingBoxOriginal = new BoundingBox();

                //Crea l'array dei bounding delle singole mesh
                meshBoundingSphere = new XModelBounding[model.Meshes.Count];

                //Modelli caricati
                countInfo.LoadedModels = 1;

                //Mesh caricate
                countInfo.LoadedMeshes = model.Meshes.Count;

                //Ancoraggio
                for (int i = 0; i < model.Bones.Count; i++)
                {
                    XModelAnchor a = new XModelAnchor(model.Bones[i].Transform, model.Bones[i].Name);
                    anchors.Add(a);
                }

                //Massimo e minimo (per la creazione della Bounding Box)
                Vector3 Max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                Vector3 Min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

                //Materiali
                materials = new Material[model.Meshes.Count][];

                //Cicla all'interno del modello
                for (int currentMesh = 0; currentMesh < model.Meshes.Count; currentMesh++)
                {
                    //Materiale della parte corrente
                    materials[currentMesh] = new Material[model.Meshes[currentMesh].MeshParts.Count];

                    //Parametri ddizionali
                    Dictionary<string, object> opaqueData = null;
                    if (model.Meshes[currentMesh].Tag != null)
                    {
                        opaqueData = model.Meshes[currentMesh].Tag as Dictionary<string, object>;
                    }

                    //Preleva il materiale dalla mesh corrente e lo aggiunge alla lista
                    //PS: Una mesh del modello può essere costituita da più parti perchè alcune faccie possono avere un altro materiale
                    for (int p = 0; p < model.Meshes[currentMesh].MeshParts.Count; p++)
                    {
                        //Preleva il valore di riflessione dalla mesh corrente (tramite gli OpaqueData nel Tag)
                        object opaqueReflection = 0.0f;
                        float opaquesReflectionFloat = 0.0f;
                        if (opaqueData != null)
                        {
                            if (opaqueData.TryGetValue("Reflection", out opaqueReflection))
                            {
                                opaquesReflectionFloat = (float)opaqueReflection;
                            }
                        }

                        //Crea il materiale della parte corrente
                        Material currentMaterialPart = new Material();
                        currentMaterialPart.MaterialFromMayaEffect(model.Meshes[currentMesh].MeshParts[p].Effect, opaquesReflectionFloat);

                        //Aggiunge il materiale della parte corrente al jagged-array
                        materials[currentMesh][p] = currentMaterialPart;

                        //Conto dei vertici caricati
                        countInfo.LoadedVerticies = countInfo.LoadedVerticies + model.Meshes[currentMesh].MeshParts[p].NumVertices;
                        countInfo.LoadedPrimitives = countInfo.LoadedPrimitives + model.Meshes[currentMesh].MeshParts[p].PrimitiveCount;
                    }

                    //Analizza le parti per calcolarne la BoundingBox
                    foreach (ModelMeshPart part in model.Meshes[currentMesh].MeshParts)
                    {
                        //Imposta la dichiarazione del VB
                        int stride = part.VertexStride;
                        int numberv = part.NumVertices;
                        VertexDeclaration test1 = part.VertexDeclaration;
                        byte[] data = new byte[stride * numberv];

                        //Preleva i dati dal VB
                        model.Meshes[currentMesh].VertexBuffer.GetData<byte>(data);

                        //Cicla all'interno dei dati e preleva quelli massimi e minimi
                        Vector3[] vector = new Vector3[data.Length / 3];

                        int counters = 0;
                        int precision = 4;
                        for (int ndx = 0; ndx < data.Length; ndx += stride)
                        {
                            float floatvaluex = (float)Math.Round(BitConverter.ToSingle(data, ndx + 0), precision, MidpointRounding.AwayFromZero);
                            float floatvaluey = (float)Math.Round(BitConverter.ToSingle(data, ndx + 4), precision, MidpointRounding.AwayFromZero);
                            float floatvaluez = (float)Math.Round(BitConverter.ToSingle(data, ndx + 8), precision, MidpointRounding.AwayFromZero);

                            if (ndx < data.Length / 3)
                            {
                                vector[counters] = new Vector3(floatvaluex, floatvaluey, floatvaluez);
                            }
                            counters = counters + 1;

                            if (floatvaluex < Min.X) Min.X = floatvaluex;
                            if (floatvaluex > Max.X) Max.X = floatvaluex;
                            if (floatvaluey < Min.Y) Min.Y = floatvaluey;
                            if (floatvaluey > Max.Y) Max.Y = floatvaluey;
                            if (floatvaluez < Min.Z) Min.Z = floatvaluez;
                            if (floatvaluez > Max.Z) Max.Z = floatvaluez;
                        }

                    }


                    //Bounding box totale
                    //Aggiunge al box precedente quello corrente in modo da crearne uno che li contenga sempre entrambi
                    BoundingBox currentBB = new BoundingBox(Min, Max);
                    BoundingSphere currentBS = BoundingSphere.CreateFromBoundingBox(currentBB);

                    Vector3 currentCenter = Vector3.Transform(currentBS.Center, absoluteBoneTransforms[model.Meshes[currentMesh].ParentBone.Index]) ;
                    float currentRadius = model.Meshes[currentMesh].BoundingSphere.Radius;

                    //Bounding spheres singole mesh
                    meshBoundingSphere[currentMesh].boundingSphereOriginal = new BoundingSphere(currentCenter, currentRadius);
                    meshBoundingSphere[currentMesh].boundingSphere = meshBoundingSphere[currentMesh].boundingSphereOriginal;
                    meshBoundingSphere[currentMesh].boundingSphere_InFrustrum = true;
                    meshBoundingSphere[currentMesh].boundingSphere_Distance = 0.0f;

                    boundingBoxOriginal = BoundingBox.CreateMerged(boundingBoxOriginal, currentBB);
                    boundingSphereOriginal = BoundingSphere.CreateMerged(boundingSphereOriginal, meshBoundingSphere[currentMesh].boundingSphereOriginal);

                    //Resetta il massimo e il minimo corrente
                    Max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                    Min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

                }

                //Dimensioni del modello
                dimensions = new Dimensions(boundingBoxOriginal.Max.X - boundingBoxOriginal.Min.X, boundingBoxOriginal.Max.Y - boundingBoxOriginal.Min.Y, boundingBoxOriginal.Max.Z - boundingBoxOriginal.Min.Z);

                //Cerca le matrici di collisione (e le aggiunge in una lista temporanea)
                List<Matrix> matrixColl = new List<Matrix>();
                int counter = 0;
                while(true)
                {
                    counter = counter + 1;
                    string nameColl = "CollisionPoint" + counter.ToString();
                    bool exist = false;
                    Matrix currentMatrix = this.GetBoneTransform(nameColl, true, out exist);
                    if (exist)
                    {
                        matrixColl.Add(currentMatrix);
                    }
                    else
                    {
                        break;
                    }
                }

                //Crea le sfere di collisioni (originali)
                if (matrixColl.Count > 0)
                {
                    collisionSpheres = new BoundingSphere[matrixColl.Count];
                    collisionSpheresOriginal = new BoundingSphere[matrixColl.Count];
                    for (int i = 0; i < matrixColl.Count; i++)
                    {
                        Vector3 loc = Vector3.Zero;
                        Quaternion quat = Quaternion.Identity;
                        Vector3 scale = Vector3.Zero;

                        //Decompone la matrice
                        if (matrixColl[i].Decompose(out scale, out quat, out loc))
                        {
                            //Se riesce a decomporre la matrice la usa per creare la sfera
                            float radius = Math.Max(Math.Max(scale.X, scale.Y), scale.Z);
                            collisionSpheresOriginal[i] = new BoundingSphere(loc, radius);
                        }
                    }
                }
                else
                {
                    //Non ha sfere di collisione, usa comunque il bounding sphere generale
                    collisionSpheres = new BoundingSphere[1];
                    collisionSpheresOriginal = new BoundingSphere[1];
                    collisionSpheresOriginal[0] = boundingSphereOriginal;
                }

                //Calcolo totale
                Core.loadedVertices = Core.loadedVertices + countInfo.LoadedVerticies;
                Core.loadedPrimitives = Core.loadedPrimitives + countInfo.LoadedPrimitives;
                Core.loadedMeshes = Core.loadedMeshes + countInfo.LoadedMeshes;
                Core.loadedModels = Core.loadedModels + countInfo.LoadedModels;

                //creazione avvenuta
                IsCreated = true;
                return true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
                return false;
            }
        }




        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (IsCreated)
            {
                //Imposta la matrice finale di posizione
                worldMatrix = Matrix.CreateScale(scale.X, scale.Y, scale.Z) *
                              Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(rotation.Y), MathHelper.ToRadians(rotation.X), MathHelper.ToRadians(rotation.Z)) *
                              Matrix.CreateTranslation(position.X, position.Y, position.Z);
                
                //BoundingSphere totale trasformata
                //Il Centro è dato dalla trasformazone tra il centro non trasformato e la matrice finale ( world * external )
                //Il Raggio è dato dal raggio non trasformato moltiplicato per la maggiore delle scalature (oppure leggere sotto)
                //Usare boundingSphere.Transform( world ) non funziona perchè vuole solo una matrice scalatura/traslazione (mentre world ha anche la rotazione)
                //PS:
                //BoneTransforms can include some scaling, so scaling the radius by our defined scale may not be sufficient.
                //Big thanks to Shawn Hargreaves (http://sharky.bluecog.co.nz/?p=116) for the following solution using localWorld.Forward.Length() instead.
                base.BoundingSphere = new BoundingSphere(Vector3.Transform(boundingSphereOriginal.Center, worldMatrix * this.externalMatrix), boundingSphereOriginal.Radius * worldMatrix.Forward.Length());

                //Sfere di collisione
                for (int i = 0; i < collisionSpheresOriginal.Length; i++)
                {
                    collisionSpheres[i].Center = Vector3.Transform(collisionSpheresOriginal[i].Center, worldMatrix * this.externalMatrix);
                    collisionSpheres[i].Radius = collisionSpheresOriginal[i].Radius * worldMatrix.Forward.Length();
                }

                //Posizione 2D
                Vector3 temp2D = Core.Graphics.GraphicsDevice.Viewport.Project(base.BoundingSphere.Center, base.ParentScene.SceneCamera.ProjectionMatrix, base.ParentScene.SceneCamera.ViewMatrix, Matrix.Identity);
                position2D.X = temp2D.X;
                position2D.Y = temp2D.Y;
                
                //Calcola se la sfera principale e quelle interne sono nel frustrum
                if ((base.ParentScene.SceneCamera.Frustrum.Contains(base.BoundingSphere) == ContainmentType.Disjoint))
                {
                    //Fuori dal frustrum
                    this.inFrustrum = false;
                }
                else
                {
                    //Dentro il frustrum
                    this.inFrustrum = true;

                    //Calcola se le singole mesh del modello sono interne al frustrum
                    for (int m = 0; m < model.Meshes.Count; m++)
                    {
                        meshBoundingSphere[m].boundingSphere.Center = Vector3.Transform(meshBoundingSphere[m].boundingSphereOriginal.Center, this.worldMatrix * this.externalMatrix);
                        meshBoundingSphere[m].boundingSphere.Radius = meshBoundingSphere[m].boundingSphereOriginal.Radius * worldMatrix.Forward.Length();
                        if (base.ParentScene.SceneCamera.Frustrum.Contains(meshBoundingSphere[m].boundingSphere) == ContainmentType.Disjoint)
                        {
                            //Mesh fuori dal frustrum
                            meshBoundingSphere[m].boundingSphere_InFrustrum = false;
                        }
                        else
                        {
                            //Mesh dentro il frustrum
                            meshBoundingSphere[m].boundingSphere_InFrustrum = true;

                            //Calcola la distanza tra la camera e la sfera (raggio) per stabilire la distanza nella nebbia
                            //La distanza è calcolata non dal centro del modello, ma dal centro del modello - il raggio della sua boundingsphere, in questo modo tiene presente anche della sua dimensione
                            meshBoundingSphere[m].boundingSphere_Distance = Vector3.Distance(base.ParentScene.SceneCamera.Position, meshBoundingSphere[m].boundingSphere.Center) - meshBoundingSphere[m].boundingSphere.Radius;
                        }
                    }
                }
            }
            base.Update(gameTime);
        }


        public override void Draw()
        {
            if (IsCreated)
            {
                //if (((lightEffect.ShadowCreationPhase) && (castShadow)) || (lightEffect.ShadowCreationPhase == false))
                {
                    //Azzera i vertici visbili
                    countInfo.CurrentVerticies = 0;
                    countInfo.CurrentPrimitives = 0;
                    countInfo.CurrentModels = 0;
                    countInfo.CurrentMeshes = 0;
                    if ((ToDraw) && (inFrustrum))
                    {
                        //Conteggio
                        countInfo.CurrentModels = countInfo.CurrentModels + 1;

                        //Salvataggio Renderstates 
                        bool old1 = Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable;
                        Blend old2 = Core.Graphics.GraphicsDevice.RenderState.SourceBlend;
                        Blend old3 = Core.Graphics.GraphicsDevice.RenderState.DestinationBlend;
                        bool old4 = Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable;
                        BlendFunction old5 = Core.Graphics.GraphicsDevice.RenderState.AlphaBlendOperation;
                        bool old6 = Core.Graphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled;
                        bool old7 = Core.Graphics.GraphicsDevice.RenderState.AlphaTestEnable;
                        CompareFunction old8 = Core.Graphics.GraphicsDevice.RenderState.AlphaFunction;
                        int old9 = Core.Graphics.GraphicsDevice.RenderState.ReferenceAlpha;
                        CullMode old10 = Core.Graphics.GraphicsDevice.RenderState.CullMode;//*/

                        //Inizio disegno
                        base.ParentScene.SceneLightEffect.DrawEffect.Begin(SaveStateMode.None);

                        //Disegna il modello
                        for (int i = 0; i < base.ParentScene.SceneLightEffect.DrawEffect.CurrentTechnique.Passes.Count; i++)
                        {
                            //Inizio passo corrente
                            base.ParentScene.SceneLightEffect.DrawEffect.CurrentTechnique.Passes[i].Begin();

                            //Cicla all'interno delle mesh
                            for (int m = 0; m < model.Meshes.Count; m++)
                            {
                                //Controlla che la mesh è dentro il frustrum
                                if (meshBoundingSphere[m].boundingSphere_InFrustrum)
                                {
                                    //Controlla se la mesh è nella parte visibile della nebbia (oltre la .End)
                                    if ((base.ParentScene.SceneLightEffect.Fog.Enable == false) || ((base.ParentScene.SceneLightEffect.Fog.Enable) && (meshBoundingSphere[m].boundingSphere_Distance < base.ParentScene.SceneLightEffect.Fog.End)))
                                    {
                                        //Calcola le luci dinamiche che effettivamento colpiscono la mesh corrente del modello corrente
                                        base.ParentScene.SceneLightEffect.CheckPointLight(meshBoundingSphere[m].boundingSphere);

                                        //Cicla all'interno delle parti e le disegna tutte
                                        for (int p = 0; p < model.Meshes[m].MeshParts.Count; p++)
                                        {
                                            //Imposta il materiale
                                            base.ParentScene.SceneLightEffect.CurrentMaterial = materials[m][p];

                                            //Imposta le matrici (è la matrice finale del modello: bone * world * external)
                                            base.ParentScene.SceneLightEffect.CurrentWorldMatrix = absoluteBoneTransforms[model.Meshes[m].ParentBone.Index] * this.worldMatrix * this.externalMatrix;

                                            //Vertici e primitive disegnate 
                                            countInfo.CurrentVerticies = countInfo.CurrentVerticies + model.Meshes[m].MeshParts[p].NumVertices;
                                            countInfo.CurrentPrimitives = countInfo.CurrentPrimitives + model.Meshes[m].MeshParts[p].PrimitiveCount;

                                            //Trasparenza
                                            if ((materials[m][p].Alpha < 1.0f) || (blendProperties == BlendMode.None))
                                            {
                                                if (blendProperties == BlendMode.AlphaBlend)
                                                {
                                                    //Blend normale (dipende dal canale alpha della texture, se il canale non c'è la trasparenza non viene applicata)
                                                    Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
                                                    Core.Graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                                                    Core.Graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                                                    Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;
                                                    Core.Graphics.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
                                                    Core.Graphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = false;
                                                    Core.Graphics.GraphicsDevice.RenderState.AlphaTestEnable = true;
                                                    Core.Graphics.GraphicsDevice.RenderState.AlphaFunction = CompareFunction.Greater;
                                                    Core.Graphics.GraphicsDevice.RenderState.ReferenceAlpha = 0;
                                                    Core.Graphics.GraphicsDevice.RenderState.CullMode = CullMode.None;
                                                }
                                                else if (blendProperties == BlendMode.Additive)
                                                {
                                                    //Blend additivo (ottimo per effetti grafici)
                                                    Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
                                                    Core.Graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                                                    Core.Graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.One;
                                                    Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;
                                                    Core.Graphics.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
                                                    Core.Graphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = false;
                                                    Core.Graphics.GraphicsDevice.RenderState.AlphaTestEnable = true;
                                                    Core.Graphics.GraphicsDevice.RenderState.AlphaFunction = CompareFunction.Greater;
                                                    Core.Graphics.GraphicsDevice.RenderState.ReferenceAlpha = 0;
                                                    Core.Graphics.GraphicsDevice.RenderState.CullMode = CullMode.None;
                                                }
                                            }
                                            else
                                            {
                                                //Nessun blend
                                                Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;
                                                Core.Graphics.GraphicsDevice.RenderState.SourceBlend = Blend.One;
                                                Core.Graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.Zero;
                                                Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
                                                Core.Graphics.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
                                                Core.Graphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = true;
                                                Core.Graphics.GraphicsDevice.RenderState.AlphaTestEnable = false;
                                                Core.Graphics.GraphicsDevice.RenderState.AlphaFunction = CompareFunction.Always;
                                                Core.Graphics.GraphicsDevice.RenderState.ReferenceAlpha = 0;
                                                //IL cull non viene impostato in modo da tenere quello definito da utente
                                            }//*/

                                            //Commit
                                            base.ParentScene.SceneLightEffect.DrawEffect.CommitChanges();

                                            //Disegna i vertici correnti (in base alla parte della mesh)
                                            Core.Graphics.GraphicsDevice.Vertices[0].SetSource(model.Meshes[m].VertexBuffer, model.Meshes[m].MeshParts[p].StreamOffset, model.Meshes[m].MeshParts[p].VertexStride);
                                            Core.Graphics.GraphicsDevice.VertexDeclaration = model.Meshes[m].MeshParts[p].VertexDeclaration;
                                            Core.Graphics.GraphicsDevice.Indices = model.Meshes[m].IndexBuffer;
                                            Core.Graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, model.Meshes[m].MeshParts[p].BaseVertex, 0, model.Meshes[m].MeshParts[p].NumVertices, model.Meshes[m].MeshParts[p].StartIndex, model.Meshes[m].MeshParts[p].PrimitiveCount);
                                        }

                                        //Mesh disegnate
                                        countInfo.CurrentMeshes = countInfo.CurrentMeshes + 1;

                                    }//boundingSphere_Distance
                                }//boundingSphere_InFrustrum
                            }//For mesh

                            //Fine passo corrente
                            base.ParentScene.SceneLightEffect.DrawEffect.CurrentTechnique.Passes[i].End();

                        }//For passi

                        //Fine disegno
                        base.ParentScene.SceneLightEffect.DrawEffect.End();

                        //Ripristino Render States
                        Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = old1;
                        Core.Graphics.GraphicsDevice.RenderState.SourceBlend = old2;
                        Core.Graphics.GraphicsDevice.RenderState.DestinationBlend = old3;
                        Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = old4;
                        Core.Graphics.GraphicsDevice.RenderState.AlphaBlendOperation = old5;
                        Core.Graphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = old6;
                        Core.Graphics.GraphicsDevice.RenderState.AlphaTestEnable = old7;
                        Core.Graphics.GraphicsDevice.RenderState.AlphaFunction = old8;
                        Core.Graphics.GraphicsDevice.RenderState.ReferenceAlpha = old9;
                        Core.Graphics.GraphicsDevice.RenderState.CullMode = old10;//*/

                    }


                    //Info totali disegnate
                    Core.currentVertices = Core.currentVertices + countInfo.CurrentVerticies;
                    Core.currentPrimitives = Core.currentPrimitives + countInfo.CurrentPrimitives;
                    Core.currentMeshes = Core.currentMeshes + countInfo.CurrentMeshes;
                    Core.currentModels = Core.currentModels + countInfo.CurrentModels;
                }
            }
            base.Draw();
        }


        public void DrawShadow(LightEffect lightEffect, Camera camera, float alpha)
        {
            //Ombra (funziona bene)

            Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            Core.Graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            Core.Graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;

            for (int i = 0; i < 3; i++)
            {
                Vector3 lightDir = Vector3.Up;
                bool isEnable = false;
                if (i == 0)
                {
                    lightDir = Vector3.Negate(lightEffect.DirectionalLight0.Direction);
                    isEnable = lightEffect.DirectionalLight0.IsEnabled;
                }
                else if (i == 1)
                {
                    lightDir = Vector3.Negate(lightEffect.DirectionalLight1.Direction);
                    isEnable = lightEffect.DirectionalLight1.IsEnabled;
                }
                else if (i == 2)
                {
                    lightDir = Vector3.Negate(lightEffect.DirectionalLight2.Direction);
                    isEnable = lightEffect.DirectionalLight2.IsEnabled;
                }

                if (isEnable)
                {
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.LightingEnabled = false;
                            effect.Alpha = alpha;
                            effect.DiffuseColor = new Vector3(.2f, .2f, .2f);
                            effect.TextureEnabled = false;

                            effect.View = camera.ViewMatrix;
                            effect.Projection = camera.ProjectionMatrix;
                            effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * this.worldMatrix * this.externalMatrix * Matrix.CreateShadow(lightDir, lightEffect.ShadowPlane);
                            effect.CommitChanges();
                        }
                        mesh.Draw(SaveStateMode.None);
                    }
                }
            }

            Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;//*/
        }

        public bool CheckCollision(XModel collisionModel)
        {
            if (this.BoundingSphere.Intersects(collisionModel.BoundingSphere))
            {
                for (int i = 0; i < this.CollisionSpheres.Length; i++)
                {
                    for (int m = 0; m < collisionModel.CollisionSpheres.Length; m++)
                    {
                        if (this.CollisionSpheres[i].Intersects(collisionModel.CollisionSpheres[m]))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CheckCollision(BoundingSphere collisionSphere)
        {
            for (int i = 0; i < this.CollisionSpheres.Length; i++)
            {
                if (this.CollisionSpheres[i].Intersects(collisionSphere))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckCollision(BoundingBox collisionBox)
        {
            for (int i = 0; i < this.CollisionSpheres.Length; i++)
            {
                if (this.CollisionSpheres[i].Intersects(collisionBox))
                {
                    return true;
                }
            }
            return false;
        }



        public Matrix GetBoneTransform(string boneName, bool exactString, out bool exist)
        {
            exist = false;
            try
            {
                if (exactString) 
                {
                    exist = true;
                    return model.Bones[boneName].Transform;
                }
                else 
                {
                    for (int i=0; i< model.Bones.Count; i++)
                    {
                        if (model.Bones[i].Name.ToString().Contains(boneName))
                        {
                            exist = true;
                            return model.Bones[i].Transform;
                        }
                    }
                    exist = false;
                    return Matrix.Identity;
                }
            }
            catch
            {
                exist = false;
                return Matrix.Identity;
            }
        }


        public Matrix GetBoneTransformRelative(string boneName, bool exactString)
        {
            try
            {
                if (exactString)
                {
                    return model.Bones[boneName].Transform * (this.worldMatrix * this.externalMatrix);
                }
                else
                {
                    for (int i = 0; i < model.Bones.Count; i++)
                    {
                        if (model.Bones[i].Name.ToString().Contains(boneName))
                        {
                            return model.Bones[i].Transform * (this.worldMatrix * this.externalMatrix);
                        }
                    }
                    return Matrix.Identity * (this.worldMatrix * this.externalMatrix);
                }
            }
            catch
            {
                return Matrix.Identity * (this.worldMatrix * this.externalMatrix);
            }
        }



        ~XModel()
        {
            //Calcolo totale
            Core.loadedVertices = Core.loadedVertices - countInfo.LoadedVerticies;
            Core.loadedPrimitives = Core.loadedPrimitives - countInfo.LoadedPrimitives;
            Core.loadedMeshes = Core.loadedMeshes - countInfo.LoadedMeshes;
        }

    }
}


