using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class motion : MonoBehaviour
{

    public int score = 0;

    bool change_level = true;

    public List<Transform> Asses;   // список жоп змейки
    public float AssDistance = 0.05f;       // дистанция между жопами
    public GameObject AssPrefab;    // ссылка на жопу
    //[Range(0,4)]
    public float speed = 0.0005f; // скорость змейки

    private Transform tmp; // трансформ отвечает за положение объекта в пространстве, его размерах, вращении..

    public UnityEvent OnEat;    // скушать яблочк
    private void Start() {
        tmp = GetComponent<Transform>();        // получаем текущие параметры объекта
    }
    
    private void Update() {     // чё меняться каждый кадр будет

        move(tmp.position + tmp.forward * speed); // forward - вернёт векторок. типа всегда вперёд 
        float angel = Input.GetAxis("Horizontal") * 4; // типа чё нажали по горизонтальной оси? если право - единицу вернёт, влево минус 1, если нихера - то ноль вернёт.
        tmp.Rotate(0, angel, 0);    // поворачиваем по x,y,z (только по игреку на ангел). оси посмотреть стоит на сцене при кручении объекта.
    }

    private void move(Vector3 NewPosition) {

        float SqrAssDistance = AssDistance * AssDistance;   // шоб квадратный корень кучу раз не считать, окда

        Transform PrevAss, Head;    // бошка хвоста и предыдущая жопа и следующая жопа
        Head = tmp; // бошка
        PrevAss = Head;
        //CurAss = Asses[0];  // первая жёпа в списке
        //int i = 0;
        foreach (var Ass in Asses) {
            if ((Ass.position - PrevAss.position).sqrMagnitude > SqrAssDistance) // магнитуд - чтобы расстояние получить между точками
            {   
                var temp = Ass.position;
                temp = Ass.position;
                Ass.position = PrevAss.position;
                PrevAss.position = temp;
            }
           else
              break;  // типа иначе, если следующая жёпа не нуждается в подцеплении, то и оставшиеся тем паче. изи.

                

        }


        tmp.position = NewPosition; // чисто для бошки. жёпы в цикле прицепили норм.
    }


    private void OnCollisionEnter (Collision collision) {       // если столкнулись. Именно OnCollisionEnter
        if (collision.gameObject.tag == "food")
        {       // c объектом типа "хавка" 
            Destroy(collision.gameObject);       //  выпиливаем объект столкновения
            var ass = Instantiate(AssPrefab);    // создаём объект-клон префаб-жёпы
            Asses.Add(ass.transform);            // и пихаем его в конец списка жёп.
                                                 //Asses.Add((Instantiate(AssPrefab)).transform);
            OnEat.Invoke();     // вызываем обработчик для события OnEat (в юнити к нему прицеплен годный музыкальный трэк и всё.)

            score += 5; // счёт увеличиваем


        }
    }

    private void OnGUI() {
        GUILayout.Label("Score: " + score);
        if (score == 140)
        {
            GUI.Label(new Rect(330, 120, 100, 20), "You win!");
            Application.Quit();
        }
    }

}
