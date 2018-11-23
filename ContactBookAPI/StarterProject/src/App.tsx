import * as React from 'react';
import Modal from 'react-responsive-modal';
import './App.css';
import MemeDetail from './components/MemeDetail';
import NavBar from './components/NavBar';
import MemeList from './components/MemeList';
import PatrickLogo from './patrick-logo.png';


interface IState {
	currentContact: any,
	contacts: any[],
	open: boolean,
	uploadFileList: any,
}

class App extends React.Component<{}, IState> {
	constructor(props: any) {
        super(props)
        this.state = {
			currentContact: {"id":0, "title":"Loading ","url":"","tags":"", "description":"","uploaded":"","width":"0","height":"0","MobilePhone":0},
			contacts: [],
			open: false,
			uploadFileList: null
		}     	
		this.selectNewMeme = this.selectNewMeme.bind(this)
		this.fetchImages = this.fetchImages.bind(this)
		this.fetchImages("")

		this.handleFileUpload = this.handleFileUpload.bind(this)
		this.uploadContact = this.uploadContact.bind(this)
	}

	public render() {
		const { open } = this.state;
		return (
		<div>
			<NavBar />
			<div className="header-wrapper">
				<div className="container header">
					<img src={PatrickLogo} height='40'/>&nbsp; PersonalContactBook &nbsp;
					<div className="btn btn-primary btn-action btn-add" onClick={this.onOpenModal}>Add Contact</div>
				</div>
			</div>
			<div className="container">
				<div className="row">
					<div className="col-7">
						<MemeDetail currentMeme={this.state.currentContact} />
					</div>
					<div className="col-5">
					<MemeList memes={this.state.contacts} selectNewMeme={this.selectNewMeme} searchByTag={this.fetchImages}/>
					</div>
				</div>
			</div>
			<Modal open={open} onClose={this.onCloseModal}>
				<form>
					<div className="form-group">
						<label>Contact Title</label>
						<input type="text" className="form-control" id="meme-title-input" placeholder="Enter Title" />
						<small className="form-text text-muted">You can edit any meme later</small>
					</div>
					<div className="form-group">
						<label>Tag</label>
						<input type="text" className="form-control" id="contact-tag-input" placeholder="Enter Tag" />
						<small className="form-text text-muted">Tag is used for search</small>
					</div>
					<div className="form-group">
						<label>Image</label>
						<input type="file" onChange={this.handleFileUpload} className="form-control-file" id="Contact-image-input" />
					</div>

					<button type="button" className="btn" onClick={this.uploadContact}>Upload</button>
				</form>
			</Modal>
		</div>
		);
	}

	// Modal open
	private onOpenModal = () => {
		this.setState({ open: true });
	  };
	
	// Modal close
	private onCloseModal = () => {
		this.setState({ open: false });
	};
	
	// Change selected meme
	private selectNewMeme(newMeme: any) {
		this.setState({
			currentContact: newMeme
		})
	}

	private fetchImages(tag: any) {
		let url = "https://msaphase2contactapi.azurewebsites.net/api/ContactItems"
		if (tag !== "") {
			url += "/tag?=" + tag
		}
		fetch(url, {
			method: 'GET'
		})
		.then(res => res.json())
		.then(json => {
			let currentContact = json[0]
			if (currentContact === undefined) {
				currentContact = {"id":0, "title":"No memes (╯°□°）╯︵ ┻━┻","url":"","tags":"try a different tag","uploaded":"","width":"0","height":"0"}
			}
			this.setState({
				currentContact,
				contacts: json
			})
		});
	}

	private handleFileUpload(fileList: any) {
		this.setState({
			uploadFileList: fileList.target.files
		})
	}

	private uploadContact() {
		const titleInput = document.getElementById("meme-title-input") as HTMLInputElement
		const tagInput = document.getElementById("contact-tag-input") as HTMLInputElement
		const imageFile = this.state.uploadFileList[0]
	
		if (titleInput === null || tagInput === null || imageFile === null) {
			return;
		}
	
		const title = titleInput.value
		const tag = tagInput.value
		const url = "https://msaphase2contactapi.azurewebsites.net/api/ContactItems/upload"
	
		const formData = new FormData()
		formData.append("Title", title)
		formData.append("Tags", tag)
		formData.append("image", imageFile)
	
		fetch(url, {
			body: formData,
			headers: {'cache-control': 'no-cache'},
			method: 'POST'
		})
		.then((response : any) => {
			if (!response.ok) {
				// Error State
				alert(response.statusText)
			} else {
				location.reload()
			}
		})
	}

	
}

export default App;
