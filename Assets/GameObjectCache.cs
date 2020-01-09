using UnityEngine;

public static class GameObjectCache {

    private static Camera _camera;
    public static Camera Camera {
        get {
            if (_camera == null){
                _camera = Camera.main;
            }
            return _camera;
        }
    }

    private static GameObject _canvas;
    public static GameObject Canvas {
        get {
            if (_canvas == null){
                _canvas = GameObject.Find("Canvas");
            }
            return _canvas;
        }
    }

}