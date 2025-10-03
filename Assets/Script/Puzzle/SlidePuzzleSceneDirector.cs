using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlidePuzzleSceneDirector : MonoBehaviour
{
    // �s�[�X
    [SerializeField] List<GameObject> pieces;
    // �Q�[���N���A���ɕ\�������{�^��
    [SerializeField] GameObject buttonRetry;
    // �V���b�t����
    [SerializeField] int shuffleCount;

    // �����ʒu
    List<Vector2> startPositions;

    // Start is called before the first frame update
    void Start()
    {
        // �����ʒu��ۑ�
        startPositions = new List<Vector2>();
        foreach (var item in pieces)
        {
            startPositions.Add(item.transform.position);
        }
        // �w��񐔃V���b�t��
        for (int i = 0; i < shuffleCount; i++)
        {
            List<GameObject> movablePieces = new List<GameObject>();

        // 0�Ɨאڂ��Ă���s�[�X�����X�g�ɒǉ�
        foreach (var item in pieces)
        {
            if (GetEmptyPiece(item))
            {
                movablePieces.Add(item);
            }
        }

        // �אڂ���s�[�X�������_���ł��ꂩ����
        int rnd = Random.Range(0, movablePieces.Count);
        GameObject piece = movablePieces[rnd];
        SwapPiece(piece, pieces[0]);
        }

        // �{�^����\��
        buttonRetry.SetActive(false);
    }

    // Update is called once per frame
    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        // �^�b�`����
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast(worldPoint, Vector2.zero);

            // �����蔻�肪������
            if (hit2d)
            {
                // �Q�[���I�u�W�F�N�g
                GameObject hitPiece = hit2d.collider.gameObject;
                // 0�Ɨאڂ��Ă���΃f�[�^������
                GameObject emptyPiece = GetEmptyPiece(hitPiece);

                // �I�񂾃s�[�X��0�Ԃ̃s�[�X����ꂩ����
                SwapPiece(hitPiece, emptyPiece);
                // �N���A����
                buttonRetry.SetActive(true);

                // �����̈ʒu�ƈႤ�s�[�X��T��
                for (int i = 0; i < pieces.Count; i++)
                {
                    // ���݂̃|�W�V����
                    Vector2 position = pieces[i].transform.position;
                    // �����ʒu�ƈ������{�^�����\��
                    if (position != startPositions[i])
                    {
                        buttonRetry.SetActive(false);
                    }
                }

                // �N���A���
                if (buttonRetry.activeSelf)
                {
                    Debug.Log("�N���A�I�I");
                    SceneManager.LoadScene("box_yuyu");
                }
            }
        }
    }

    // �����̃s�[�X��0�Ԃ̃s�[�X�Ɨאڂ��Ă�����0�Ԃ̃s�[�X��Ԃ�
    GameObject GetEmptyPiece(GameObject piece)
    {
        // 2�_�Ԃ̋�������
        float dist =
            Vector2.Distance(piece.transform.position, pieces[0].transform.position);

        // ������1�Ȃ�0�Ԃ̃s�[�X��Ԃ��i2�ȏ㗣��Ă�����A�΂߂̏ꍇ��1���傫�������ɂȂ�j
        if (dist == 1)
        {
            return pieces[0];
        }

        return null;
    }

    // 2�̃s�[�X�̈ʒu����ꂩ����
    void SwapPiece(GameObject pieceA, GameObject pieceB)
    {
        // �ǂ��炩��null�Ȃ珈�������Ȃ�
        if (pieceA == null || pieceB == null)
        {
            return;
        }

        // A��B�̃|�W�V��������ꂩ����
        Vector2 position = pieceA.transform.position;
        pieceA.transform.position = pieceB.transform.position;
        pieceB.transform.position = position;
    }
    // ���g���C�{�^��
    public void OnClickRetry()
    {
        SceneManager.LoadScene("SlidePuzzleScene");
    }
}