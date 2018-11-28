using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {
    public List<DefensePoint> newDeffensePoints = new List<DefensePoint>();
    List<DefensePoint> RightDeffensePoints = new List<DefensePoint>();
    List<DefensePoint> LeftDeffensePoints = new List<DefensePoint>();
    public List<Enemy> freeEnemys = new List<Enemy>();

    public float soldierOffset;
    public Transform throne;
    public float maxEnemySearch, minEnemySearch;
    public static BattleManager instance;

    void Awake() {
        instance = this;
        foreach(DefensePoint point in newDeffensePoints) {
            if(point.gate != null) {
                if(point.gate.position.x > 0) {
                    if(RightDeffensePoints.Count == 0) {
                        RightDeffensePoints.Add(point);
                    }
                    else {
                        for(int i = 0; i < RightDeffensePoints.Count; i++) {
                            if(RightDeffensePoints[i].gate.position.x < point.gate.position.x) {
                                RightDeffensePoints.Insert(i, point);
                                break;
                            }
                            else if(i == 0) {
                                RightDeffensePoints.Add(point);
                            }
                        }
                    }

                }
                else {
                    if(LeftDeffensePoints.Count == 0) {
                        LeftDeffensePoints.Add(point);
                    }
                    else {
                        for(int i = 0; i < LeftDeffensePoints.Count; i++) {
                            if(LeftDeffensePoints[i].gate.position.x > point.gate.position.x) {
                                LeftDeffensePoints.Insert(i, point);
                                break;
                            }
                            else if(i == LeftDeffensePoints.Count - 1) {
                                LeftDeffensePoints.Add(point);
                            }
                        }
                    }
                }
            }
        }


    }

    public Transform EnemyGetTarget(float _myPosX) {
        if(_myPosX >= 0) {
            for(int i = 0; i < RightDeffensePoints.Count; i++) {
                if(!RightDeffensePoints[i].castleGate.isOpen && RightDeffensePoints[i].gate.position.x < _myPosX) {
                    return (RightDeffensePoints[i].gate.transform);
                }
            }
            return (throne);
        }
        else {
            for(int i = 0; i < LeftDeffensePoints.Count; i++) {
                if(!LeftDeffensePoints[i].castleGate.isOpen && LeftDeffensePoints[i].gate.position.x > _myPosX) {
                    return (LeftDeffensePoints[i].gate.transform);
                }
            }
            return (throne);
        }
    }

    //finding a path to a enemy is there is none go to closest reachable gate
    public Transform AllyGetTarget(float _myPosX, Allie _me, Transform _myCurrentTarget) {
        Transform _bestGate = null;
        float _min = minEnemySearch;
        float _max = maxEnemySearch;

        if(_myPosX >= 0) {
            for(int i = 0; i < RightDeffensePoints.Count; i++) {
                if(!RightDeffensePoints[i].castleGate.isOpen) {
                    if(_myPosX > RightDeffensePoints[i].gate.position.x) {
                        if(i == 0) {
                            _bestGate = RightDeffensePoints[i].frontGate;
                            _min = RightDeffensePoints[i].frontGate.position.x;
                        }
                        else {
                            _min = RightDeffensePoints[i].gate.position.x;
                        }
                        break;
                    }
                    else {
                        _bestGate = RightDeffensePoints[i].backGate;
                        _max = RightDeffensePoints[i].backGate.position.x;
                    }
                }
                else {
                    if(i == 0) {
                        _max = maxEnemySearch;
                        _bestGate = RightDeffensePoints[i].frontGate;
                    }
                }
            }
            if(_min < 0) {
                for(int i = 0; i < LeftDeffensePoints.Count; i++) {
                    if(!LeftDeffensePoints[i].castleGate.isOpen) {
                        _min = LeftDeffensePoints[i].backGate.position.x;
                    }
                    else {
                        if(i - 1 > 0) {
                            _min = LeftDeffensePoints[i - 1].backGate.position.x;
                        }
                        else {
                            _min = minEnemySearch;
                        }
                    }
                }
            }

        }
        else {
            for(int i = 0; i < LeftDeffensePoints.Count; i++) {
                if(!LeftDeffensePoints[i].castleGate.isOpen) {
                    if(_myPosX < LeftDeffensePoints[i].gate.position.x) {
                        if(i == 0) {
                            _bestGate = LeftDeffensePoints[i].frontGate;
                            _max = LeftDeffensePoints[i].frontGate.position.x;
                        }
                        else {
                            _max = LeftDeffensePoints[i].gate.position.x;
                        }
                        break;
                    }
                    else {
                        _bestGate = LeftDeffensePoints[i].backGate;
                        _min = LeftDeffensePoints[i].backGate.position.x;
                    }
                }
                else {
                    if(i == 0) {
                        _min = minEnemySearch;
                        _bestGate = LeftDeffensePoints[i].frontGate;
                    }
                }
            }
            if(_max > 0) {
                for(int i = 0; i < RightDeffensePoints.Count; i++) {
                    if(!RightDeffensePoints[i].castleGate.isOpen) {
                        _max = RightDeffensePoints[i].backGate.position.x;
                    }
                    else {
                        if(i - 1 > 0) {
                            _max = RightDeffensePoints[i - 1].backGate.position.x;
                        }
                        else {
                            _max = maxEnemySearch;
                        }
                    }
                }
            }
        }

        if(_myCurrentTarget != null && _myCurrentTarget.tag == "Enemy" && _myCurrentTarget.transform.position.x < _max && _myCurrentTarget.transform.position.x > _min) {
            return (_myCurrentTarget);
        }
        else if(freeEnemys.Count > 0) {
            Enemy _nextEnemt = GetClosest(_max, _min);
            if(_nextEnemt != null) {
                return (_nextEnemt.transform);
            }
        }

        return (_bestGate);
    }

    Enemy GetClosest(float maxDistance, float minDistance) {
        Enemy closest = null;
        foreach(Enemy t in freeEnemys) {
            if(t != null && t.transform.position.x < maxDistance && t.transform.position.x > minDistance) {
                if(closest == null) {
                    closest = t;
                }
                else if(t.attackingSoldiers.Count < closest.attackingSoldiers.Count || t.attackingSoldiers.Count == closest.attackingSoldiers.Count && t.transform.position.x > closest.transform.position.x) {
                    closest = t;
                }
            }
        }
        return (closest);
    }

    [System.Serializable]
    public struct DefensePoint {
        public Transform frontGate;
        public Transform backGate;
        public Transform gate;
        public CastleGate castleGate;
    }
}
