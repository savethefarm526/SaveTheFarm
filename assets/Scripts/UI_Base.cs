using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour {
	public void init_Node(Transform tf){
		this.node_Asset (tf.name, tf.gameObject);
		Button btn = tf.GetComponent<Button> ();
		if (btn!=null) {
			btn.onClick.AddListener (() => {
				this.button_Click (btn.name, btn.gameObject);
			});
		}
		for (int i = 0; i < tf.childCount; i++)
			this.init_Node (tf.GetChild (i));
	}
	public virtual void node_Asset(string name,GameObject obj){
	}
	public virtual void button_Click(string name,GameObject obj){
	}
}
