import { createContext, useState } from 'react'
import LoginForm from '../forms/LoginForm'
import RegisterForm from '../forms/RegisterForm';
import ForgotPasswordForm from '../forms/ForgotPasswordForm';
import GroupFoundationForm from '../forms/GroupFoundationForm';
import PasswordResetForm from '../forms/PasswordResetForm';
import Error404Page from '../../pages/Error404Page';
import MessagesPage from '../../pages/MessagesPage';
import ThanksPage from '../../pages/ThanksPage';
import GroupProfile from '../profiles/GroupProfile';
import PersonProfile from '../profiles/PersonProfile';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import FriendshipReqTile from '../friendships/FriendshipReqTile';
import SearchPage from '../searchPeople/SearchPage';
import {HubConnectionBuilder} from "@microsoft/signalr";
import Error500Page from '../../pages/Error500Page';
import Error403Page from '../../pages/Error403Page';

var messagingContextInterface = {
    context: {
        friendsData: [],
        groupsData: [],
        connection: new HubConnectionBuilder()
    },
    setContext: () => { }
}

export const UserDataContext = createContext({ ...messagingContextInterface });

export default function App() {

    const [context, setContext] = useState({ ...messagingContextInterface.context });

    try {

        return (
            <Router >
                <Routes>
                    <Route path="/" Component={LoginForm} />
                    <Route path="/register" Component={RegisterForm} />
                    <Route path="/forgot-password" Component={ForgotPasswordForm} />
                    <Route path="/group-foundation" Component={GroupFoundationForm} />
                    <Route path="/password-reset" Component={PasswordResetForm} />

                    <Route path="/error-404" Component={Error404Page} />
                    <Route path="/error-500" Component={Error500Page} />
                    <Route path="/error-403" Component={Error403Page} />
                    <Route path="/thanks" Component={ThanksPage} />

                    <Route path="/messages" Component={MessagesPage} />
                    <Route path="/requests" Component={FriendshipReqTile} />
                    <Route path="/group-profile" Component={GroupProfile} />
                    <Route path="/person-profile" Component={PersonProfile} />

                    <Route path="/search" Component={SearchPage} />
                </Routes>
            </ Router>
        );
    } catch (e) {
        e.console.log(e.message);
    }   
}
