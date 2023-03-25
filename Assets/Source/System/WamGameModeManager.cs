//------------------------------------------------------------------------------//
//!	@file   WamGameModeManager.cs
//!	@brief	�Q�[�����[�h�Ǘ��\�[�X
//!	@author	���Q��
//!	@date	2023/03/17
//------------------------------------------------------------------------------//


//======================================//
//				Include					//
//======================================//

using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


//######################################################################################//
//!								�Q�[�����[�h�Ǘ��N���X
//######################################################################################//

public class WamGameModeManager : MonoBehaviour
{
    //======================================//
    //				 �f���Q�[�g				//
    //======================================//

    /* �f���Q�[�g�錾�i�X�R�A�ϓ��j */
    public delegate void OnScoreUpdateDelegate( uint Score );
    /* �f���Q�[�g��`�i�X�R�A�ϓ��j */
    public OnScoreUpdateDelegate OnScoreUpdate;

    /* �f���Q�[�g�錾�i�������@�����񐔕ϓ��j */
    public delegate void OnMoleSlapCountUpdateDelegate( uint Count );
    /* �f���Q�[�g��`�i�������@�����񐔕ϓ��j */
    public OnMoleSlapCountUpdateDelegate OnMoleSlapCountUpdate;


    //======================================//
    //		    �v���C�x�[�g�ϐ�        	//
    //======================================//

    /* ���݂̃X�R�A */
    private uint mCurrentScore;

    /* ���݂̂������@������ */
    private uint mCurrentSlapCount;


    //======================================//
    //		    �p�u���b�N�֐�             	//
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	����������
    //------------------------------------------------------------------------------//
    public void Initialize( )
    {
        /* �e��ϐ������� */
        this.mCurrentScore      = 0;    /* ���݂̃X�R�A */
        this.mCurrentSlapCount  = 0;    /* ���݂̂������@������ */
    }

    //------------------------------------------------------------------------------//
    //! @brief	����݂̂̏��������s����
    //------------------------------------------------------------------------------//
    public void ExecFirstProcess( )
    {
        /* ���݃X�R�A�����������ꂽ���Ƃ�ʒm */
        this.OnScoreUpdate( 0 );

        /* ���݂������@�����񐔂����������ꂽ���Ƃ�ʒm */
        this.OnMoleSlapCountUpdate( 0 );
    }

    //------------------------------------------------------------------------------//
    //! @brief	�����J�n���ɌĂ΂�� 
    //------------------------------------------------------------------------------//
    public void Awake( )
    {

    }

    //------------------------------------------------------------------------------//
    //! @brief	����X�V�����̒��O�ɌĂ΂��
    //------------------------------------------------------------------------------//
    public void Start( )
    {

    }

    //------------------------------------------------------------------------------//
    //! @brief	�X�V����
    //------------------------------------------------------------------------------//
    public void Update( )
    {
        /* �������ݎ��Ԃ�0�b�ȉ��Ȃ� */
        if ( WamGameInstanceManager.GetInstance( ).GetTimeManagerInstance( ).IsTimeOver )
        {
            return;
        }

        /* �����_���ł�����𐶐����� */
        WamGameInstanceManager.GetInstance( ).GetMoleSpawnManagerInstance( ).SpawnRandomMole( );
    }

    //------------------------------------------------------------------------------//
    //! @brief	�X�R�A�����Z����
    //------------------------------------------------------------------------------//
    public void AddScore( ushort Score )
    {
        /* ���݂̃X�R�A�ɓ|���ꂽ������̃X�R�A�����Z */
        this.mCurrentScore += Score;

        /* �X�R�A���ϓ��������Ƃ�ʒm */
        this.OnScoreUpdate( this.mCurrentScore );

        /* ���݂̂������@�����񐔂����Z */
        this.mCurrentSlapCount++;

        /* �������@�����񐔂��ϓ��������Ƃ�ʒm */
        this.OnMoleSlapCountUpdate( this.mCurrentSlapCount );
    }
}
