//------------------------------------------------------------------------------//
//!	@file   WamEnemySpawnManager.cs
//!	@brief	�G�����Ǘ��\�[�X
//!	@author	���Q��
//!	@date	2023/03/20
//------------------------------------------------------------------------------//


//======================================//
//				Include					//
//======================================//

using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;


//######################################################################################//
//!								�G�����Ǘ��N���X
//######################################################################################//

public class WamMoleSpawnManager : MonoBehaviour
{
    //======================================//
    //		�v���C�x�[�g�V���A���C�Y�ϐ�	//
    //======================================//

    /* �v���C�G���A */
    [field: SerializeField, Label( "�v���C�G���A" ), Tooltip( "�����炪�o������͈́A�y�ѐ�������������v���n�u���A�^�b�`����e�I�u�W�F�N�g" )]
    private GameObject mpObjPlayArea;

    /* �v���n�u�z��i������j */
    [field: SerializeField, Label( "�v���n�u�i������j" ), Tooltip( "���I�����_���������������̃v���n�u�S" )]
    private GameObject[] mpObjMoles;

    /* ������̐����C���^�[�o���i�ŏ��j */
    [field: SerializeField, Label( "�����C���^�[�o���i�ŏ��j" ), Tooltip( "�����炪���������܂łɑҋ@���K�v�ȍŏ��C���^�[�o������" ), Range( 0.0f , 100.0f )]
    private float mSpawnIntavalMin;

    /* ������̐����C���^�[�o���i�ő剄���j */
    [field: SerializeField, Label( "�����C���^�[�o���i�ő剄���j" ), Tooltip( "�����炪���������܂łɑҋ@���K�v�ȍő剄���C���^�[�o�����ԂŁA�ŏ����Ԃɉ��Z�����" ), Range( 0.0f , 100.0f )]
    private float mSpawnIntavalMaxExt;

    /* ������Ԃ̍ŏ����������Ԋu */
    [field: SerializeField, Label( "�ŏ����������Ԋu" ), Tooltip( "�O�񐶐����ꂽ�����炩��󂯂�ŏ������Ԋu" ), Range( 0.0f , 10000.0f )]
    private float mSpawnDistanceMin;


    //======================================//
    //		    �v���C�x�[�g�ϐ�        	//
    //======================================//

    /* �����琶���܂ł̌��݂̃C���^�[�o������ */
    private float mCurrentSpawnIntervalTime;

    /* �����琶���܂ł̃����_�������C���^�[�o������ */
    private float mRandomExtSpawnIntervalTime;

    /* ���O�ɐ������ꂽ������ԂƂ̋��� */
    private float mSpawnDistance;

    /* �������������v���n�u�̃����_���I�萔�l */
    private byte mRandomSelect;

    /* ������̐������W */
    private Vector2 mSpawnLocation;

    /* �v���C�G���A�̒Z�`�ϊ���� */
    private RectTransform mPlayAreaRect;

    /* ��������������v���n�u */
    private GameObject mpCurrentMole;


    //======================================//
    //		    �p�u���b�N�֐�             	//
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	�����J�n���ɌĂ΂�� 
    //------------------------------------------------------------------------------//
    public void Awake( )
    {

    }

    //------------------------------------------------------------------------------//
    //! @brief	����X�V�����̒��O�ɌĂ΂��
    //------------------------------------------------------------------------------//
    public void Start()
    {
        /* �v���C�G���A�̒Z�`�ϊ������擾 */
        if ( this.mpObjPlayArea != null )
        {
            this.mPlayAreaRect = this.mpObjPlayArea.GetComponent<RectTransform>( );
        }

        /* ���s�ɕK�v�Ȑݒ�������m�F����i���b�Z�[�W�t���j */
        this.CheckRequiredConditions( bMessage: true );
    }

    //------------------------------------------------------------------------------//
    //! @brief	�X�V����
    //------------------------------------------------------------------------------//
    public void Update()
    {
        
    }

    //------------------------------------------------------------------------------//
    //! @brief	����������
    //------------------------------------------------------------------------------//
    public void Initialize( )
    {
        /* �e��ϐ������� */
        this.mRandomSelect              = 0;            /* �������������v���n�u�̃����_���I�萔�l */
        this.mCurrentSpawnIntervalTime  = 0.0f;         /* �����琶���܂ł̌��݂̃C���^�[�o������ */
        this.mSpawnDistance             = 0.0f;         /* ���O�ɐ������ꂽ������ԂƂ̋��� */
        this.mpCurrentMole              = null;         /* ��������������v���n�u */
        this.mSpawnLocation             = Vector2.zero; /* ������̐������W */

        /* ���O�ɗ����̃V�[�h�l��ύX���āA���������̏������s�� */
        Random.InitState( System.DateTime.Now.Millisecond );

        /* �����琶���܂ł̃����_�������C���^�[�o�����Ԃ��擾���� */
        this.mRandomExtSpawnIntervalTime = Random.Range( 0.0f , this.mSpawnIntavalMaxExt );
    }

    //------------------------------------------------------------------------------//
    //! @brief	������������_���ȍ��W�A��ނŐ�������
    //------------------------------------------------------------------------------//
    public bool SpawnRandomMole( )
    {
        /* ���s�ɕK�v�ȏ������ݒ肳��Ă��Ȃ��ꍇ�A���O��\�����Đ����ł��Ȃ��������ʂ�Ԃ� */
        if ( !this.CheckRequiredConditions( ) )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Failed , "WamMoleSpawnManager" , "Mole spawn conditions is not match" );
            return false;
        }

        /* �����琶���܂ł̌��݂̃C���^�[�o�����Ԃ��A�w�莞�Ԃɓ��B���Ă��Ȃ��ꍇ */
        if ( this.mCurrentSpawnIntervalTime < ( this.mSpawnIntavalMin + this.mRandomExtSpawnIntervalTime ) )
        {
            /* �C���^�[�o�����Ԃ����Z���A�܂������ł��Ȃ����ʂ�Ԃ� */
            this.mCurrentSpawnIntervalTime += Time.deltaTime;
            return false;
        }

        /* ���O�ɗ����̃V�[�h�l��ύX���āA���������̏������s�� */
        Random.InitState( System.DateTime.Now.Millisecond );

        /* ������𐶐�������W���v���C�G���A�����烉���_���Ŏ擾���� */
        this.GetRandomSpawnLocation( );

        /* ���O�ɐ������ꂽ�����炪����ꍇ�A���̂�����ƌ��݃����_���Ŏ擾�������W�Ƃ̋����𒲂ׁA�w�苗���ȓ��̏ꍇ�͍ēx�����_���ō��W����点�� */
        if ( !this.CheckSpawnDistance( ) )
        {
            return false;
        }

        /* ������v���n�u�z�񂩂烉���_���Ő�������������I��A�v���n�u�����݂��Ȃ��ꍇ�͍ēx�����_���I�肷�� */
        if ( !this.GetRandomMole( ) )
        {
            return false;
        }

        /* ������v���n�u�𐶐����A���s���Ă���ꍇ�͍ēx�������[�`���ɖ߂� */
        if ( !this.CreateMole( ) )
        {
            return false;
        }

        /* �����琶���܂ł̌��݂̃C���^�[�o�����Ԃ������� */
        this.mCurrentSpawnIntervalTime = 0.0f;

        /* �����琶���܂ł̃����_�������C���^�[�o�����Ԃ��擾���� */
        this.mRandomExtSpawnIntervalTime = Random.Range( 0.0f , this.mSpawnIntavalMaxExt );

        //  WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Success , "WamMoleSpawnManager" , "Mole spawned to (" + this.mSpawnLocation.x + " , " + this.mSpawnLocation.y + ")" );

        /* ������v���n�u�𐶐��o�������ʂ�Ԃ� */
        return true;
    }


    //======================================//
    //		    �v���C�x�[�g�֐�            //
    //======================================//

    //------------------------------------------------------------------------------//
    //! @brief	���s�ɕK�v�Ȑݒ�������m�F����
    //!
    //! @param	bMessage	�f�o�b�O���b�Z�[�W��\�����邩�ǂ���
    //!
    //! @return ���s�ɕK�v�ȏ������ݒ肳��Ă������ǂ���
    //------------------------------------------------------------------------------//
    private bool CheckRequiredConditions( bool bMessage = false )
    {
        /* ������v���n�u�S����Ȃ� */
        if ( this.mpObjMoles.Length <= 0 )
        {
            /* �f�o�b�O���b�Z�[�W��\������ꍇ�̂ݕ\������ */
            if ( bMessage )
            {
                WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamMoleSpawnManager" , "Mole prefab is null" );
            }

            /* ���s�ɕK�v�ȏ������ݒ肳��Ă��Ȃ����ʂ�Ԃ� */
            return false;
        }

        /* �v���C�G���A����Ȃ� */
        if ( this.mpObjPlayArea == null )
        {
            /* �f�o�b�O���b�Z�[�W��\������ꍇ�̂ݕ\������ */
            if ( bMessage )
            {
                WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamMoleSpawnManager" , "PlayArea is null" );
            }

            /* ���s�ɕK�v�ȏ������ݒ肳��Ă��Ȃ����ʂ�Ԃ� */
            return false;
        }

        /* �v���C�G���A�̒Z�`�ϊ���񂪋�Ȃ� */
        if ( this.mPlayAreaRect == null )
        {
            /* �f�o�b�O���b�Z�[�W��\������ꍇ�̂ݕ\������ */
            if ( bMessage )
            {
                WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamMoleSpawnManager" , "PlayArea RectTransform component is null" );
            }

            /* ���s�ɕK�v�ȏ������ݒ肳��Ă��Ȃ����ʂ�Ԃ� */
            return false;
        }

        /* ���s�ɕK�v�ȑS�Ă̏������ݒ肳��Ă��錋�ʂ�Ԃ� */
        return true;
    }

    //------------------------------------------------------------------------------//
    //! @brief	������𐶐�������W���v���C�G���A�����烉���_���Ŏ擾����
    //------------------------------------------------------------------------------//
    private void GetRandomSpawnLocation( )
    {
        /* ������̐������W�������_���Ŏ擾���� */
        this.mSpawnLocation.x = Random.Range( ( this.mpObjPlayArea.transform.position.x - ( this.mPlayAreaRect.rect.width / 2 ) ) ,
                                                ( this.mpObjPlayArea.transform.position.x + ( this.mPlayAreaRect.rect.width / 2 ) ) );
        this.mSpawnLocation.y = Random.Range( ( this.mpObjPlayArea.transform.position.y - ( this.mPlayAreaRect.rect.height / 2 ) ) ,
                                                ( this.mpObjPlayArea.transform.position.y + ( this.mPlayAreaRect.rect.height / 2 ) ) );
    }

    //------------------------------------------------------------------------------//
    //! @brief	���O�ɐ������ꂽ�����炪����ꍇ�A���̂�����ƌ��݃����_���Ŏ擾�������W�Ƃ̋����𒲂ׂ�
    //------------------------------------------------------------------------------//
    private bool CheckSpawnDistance( )
    {
        /* ���O�ɐ����ς݂̂�����v���n�u�����݂��Ȃ��ꍇ�A�����̔�r�̕K�v���������ʂ�Ԃ� */
        if ( this.mpCurrentMole == null )
        {
            return true;
        }

        /* ������Ԃ̌��݂̐����������擾 */
        this.mSpawnDistance = Vector2.Distance( this.mpCurrentMole.transform.position , new Vector2( this.mSpawnLocation.x , this.mSpawnLocation.y ) );

        /* �O�񐶐�����������ƁA���񐶐�����\��̂�����̋������A�w�苗�������̏ꍇ */
        if ( this.mSpawnDistance < this.mSpawnDistanceMin )
        {
            /* ������x�����_���ō��W����点���� */
            return false;
        }

        /* �O�񐶐�������ƍ��񃉃��_�����W�̋������A�w��ȏ�̋����ł��������ʂ�Ԃ� */
        return true;
    }

    //------------------------------------------------------------------------------//
    //! @brief	������v���n�u�z�񂩂烉���_���Ő�������������I�肷��
    //------------------------------------------------------------------------------//
    private bool GetRandomMole( )
    {
        /* �������������v���n�u�������_���őI�� */
        this.mRandomSelect = (byte)Random.Range( 0 , this.mpObjMoles.Length );

        /* ������v���n�u����Ȃ� */
        if ( this.mpObjMoles[this.mRandomSelect] == null )
        {
            /* �f�o�b�O���b�Z�[�W��\������ */
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Error , "WamMoleSpawnManager" , "Moles prefab array num " + this.mRandomSelect + " is null" );

            /* ������x�����_���ł��������点���� */
            return false;
        }

        /* �����_���őI�肵�������炪���݂������ʂ�Ԃ� */
        return true;
    }

    //------------------------------------------------------------------------------//
    //! @brief	������v���n�u�𐶐�����
    //------------------------------------------------------------------------------//
    private bool CreateMole( )
    {
        /* �����_���Ŏ擾�������W�Ƀ����_���őI�肵��������v���n�u�𐶐����� */
        this.mpCurrentMole = Instantiate( this.mpObjMoles[this.mRandomSelect] , new Vector2( this.mSpawnLocation.x , this.mSpawnLocation.y ) , Quaternion.identity , this.mpObjPlayArea.transform );

        /* ������v���n�u�̐������������Ă��Ȃ��ꍇ�A�G���[���O���o���Ď��s�������ʂ�Ԃ� */
        if ( this.mpCurrentMole == null )
        {
            WamGameInstanceManager.GetInstance( ).GetDebugManagerInstance( ).ShowDebugLogTemplate( WamDebugManager.EWamLogType.Failed , "WamMoleSpawnManager" , "Mole prefab create failed" );
            return false;
        }

        /* ������v���n�u�̐����������������ʂ�Ԃ� */
        return true;
    }
}
