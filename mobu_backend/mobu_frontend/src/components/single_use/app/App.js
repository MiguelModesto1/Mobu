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

export default function App(){
    try {
        return(
            <Router >
                <Routes>
                    <Route path="/" Component={LoginForm} />
                    <Route path="/register" Component={RegisterForm} />
                    <Route path="/forgot-password" Component={ForgotPasswordForm} />
                    <Route path="/group-foundation" Component={GroupFoundationForm}/>
                    <Route path="/password-reset" Component={PasswordResetForm} />

                    <Route path="/error-404" Component={Error404Page} />
                    {/*<Route path="/game-tile" Component={GameTilePage} />*/}
                    <Route path="/messages" Component={MessagesPage} />
                    <Route path="/thanks" Component={ThanksPage} />

                    <Route path="/requests" Component={FriendshipReqTile}/>
                    {/*<Route path="/game" Component={Game} />
                    <Route path="/opponent-choice" Component={OpponentTile} />*/}
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
