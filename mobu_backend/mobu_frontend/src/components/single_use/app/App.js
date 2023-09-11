import React, { useEffect } from 'react';
import Button from '../../modular/Button';
import ChooseOpponentItem from '../../modular/ChooseOpponentItem';
import ClickableIcon from '../../modular/ClickableIcon';
import GameTile from '../../modular/GameTile';
import GroupMemberItem from '../../modular/GroupMemberItem';
import Input from '../../modular/Input';
import Link from '../../modular/Link';
import MenuItem from '../../modular/MenuItem';
import PersonGroupFoundItem from '../../modular/PersonGroupFoundItem';
import PersonGroupItem from '../../modular/PersonGroupItem';
import MessageHeaderBar from '../messageHeaderBar/MessageHeaderBar';
import GroupProfile from '../profiles/GroupProfile'
import PersonProfile from '../profiles/PersonProfile';
import OwnerOptionMenu from '../optionMenus/OwnerOptionMenu';
import GroupContextMenu from '../optionMenus/GroupContextMenu';
import FriendContextMenu from '../optionMenus/FriendContextMenu';
import ForgotPasswordForm from '../forms/ForgotPasswordForm';
import LoginForm from '../forms/LoginForm';
import RegisterForm from '../forms/RegisterForm';
import MessageFooterBar from '../messageFooterBar/MessageFooterBar';
import SearchBar from '../searchPeople/SearchBar';

export default function App(){

    function handleClick(prm){
        //window.location.assign("https://www.google.com");
        window.location.assign("./assets/images/logo512.png");
    }

    return(
        <>
            <SearchBar />
        </>
    );

    
}
