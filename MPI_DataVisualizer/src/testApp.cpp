
#include "testApp.h"

//--------------------------------------------------------------
void testApp::setup(){
    
    dBase.loadCities("cities.csv");
    dBase.loadSample(2000, "2000.csv");
    dBase.loadSample(2005, "2005.csv");
    dBase.loadSample(2010, "2010.csv");
    dBase.interpolateDataBase();
    
//	cout << A.data[0][0] << endl;
}

//--------------------------------------------------------------
void testApp::update(){
	
}

//--------------------------------------------------------------
void testApp::draw(){
//	
//	ofSetColor(0);
//	ofFill();
//	ofDrawBitmapString("CSV FILE", 200, 50);
//	
//	// Check how many rows exist.
//	ofDrawBitmapString("csv rows: " + ofToString(csv.numRows), 200, 70);
//	// Check how many column exist.
//	// For that we reat the first line from CSV. (data[0])
//	ofDrawBitmapString("csv cols: " + ofToString(csv.data[0].size()), 200, 90);
//	
//	// Print out all rows and cols.
//	for(int i=0; i<csv.numRows; i++) {
//		for(int j=0; j<csv.data[i].size(); j++) {
//			ofDrawBitmapString(csv.data[i][j], 200+j*100, 150+i*20);
//		}
//	}
//	
//	ofDrawBitmapString("CSV VECTOR STRING", 200, 350);
//	// Read a CSV row as simple String.
//	vector<string> dataExample = csv.getFromString("0x11120119][100][40][445][23][543][46][24][56][14][964][12", "][");
//	//cout << "dataExample[0]" << dataExample[0] << endl;
//	
//	// Print the hole CSV data string to console.
//	for(int i=0; i<dataExample.size(); i++) {
//		ofDrawBitmapString("[" + ofToString(i) + "]: " + ofToString(dataExample[i]), 200, 370+i*20 );
//	}
	
}

//--------------------------------------------------------------
void testApp::keyPressed(int key){
	
//	// Set a 
//    // Get a specific value as inegert, float, String etc.
//	csv.setInt(0, 0, 2305);
//	cout << "getInt: " << csv.getInt(0, 0) << endl;
//	csv.setFloat(0, 1, 23.666);
//	cout << "getFloat: " << csv.getFloat(0, 1) << endl;
//	csv.setString(0, 2, "helloworld");
//	cout << "getString: " << csv.getString(0, 2) << endl;
//	csv.setBool(0, 3, true);
//	cout << "getBool: " << csv.getBool(0, 3) << endl;
	
}

//--------------------------------------------------------------
void testApp::keyReleased(int key){
}

//--------------------------------------------------------------
void testApp::mouseMoved(int x, int y ){
	
}

//--------------------------------------------------------------
void testApp::mouseDragged(int x, int y, int button){
	
}

//--------------------------------------------------------------
void testApp::mousePressed(int x, int y, int button){
	
}

//--------------------------------------------------------------
void testApp::mouseReleased(int x, int y, int button){
	
}

//--------------------------------------------------------------
void testApp::windowResized(int w, int h){
	
}

//--------------------------------------------------------------
void testApp::gotMessage(ofMessage msg){
	
}

//--------------------------------------------------------------
void testApp::dragEvent(ofDragInfo dragInfo){ 
	
}
